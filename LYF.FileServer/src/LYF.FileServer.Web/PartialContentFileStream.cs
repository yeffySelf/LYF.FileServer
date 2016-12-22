using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LYF.FileServer.Web
{
    /// <summary>
    /// 采用组合模式，定义了新的FileStream，较官方的FileStream可以定义起止范围。FileStreamResult主要依赖CopyToAsync方法
    /// 官方的CopyToAsync采用unsafe方式进行全部拷贝，这里采用循环里ReadAsync和WriteAsync方法实现。
    /// </summary>
    public class PartialContentFileStream : Stream
    {
        private readonly long _start;
        private readonly long _end;
        private FileStream _fileStream;

        public PartialContentFileStream(FileStream fileStream, long from, long to)
        {
            _fileStream = fileStream;
            _start = from;
            _end = to;
            if (_start > 0)
            {
                _fileStream.Seek(_start, SeekOrigin.Begin);
            }
        }

        public override bool CanRead
        {
            get
            {
                return _fileStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return _fileStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return _fileStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return _fileStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return _fileStream.Position - _start;
            }
            set
            {
                _fileStream.Position = value + _start;
            }
        }

        public override void Flush()
        {
            _fileStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if (_fileStream.Position + count + offset > _end + 1)
            {
                byteCountToRead = (int)(_end - _fileStream.Position - offset) + 1;
            }
            var result = _fileStream.Read(buffer, offset, byteCountToRead);
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _fileStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _fileStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            int byteCountToWrite = count;
            if (_fileStream.Position + count + offset > _end + 1)
            {
                byteCountToWrite = (int)(_end - _fileStream.Position - offset) + 1;
            }
            _fileStream.Write(buffer, offset, byteCountToWrite);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int byteCountToRead = count;
            if (_fileStream.Position + count + offset > _end + 1)
            {
                byteCountToRead = (int)(_end - _fileStream.Position - offset) + 1;
            }
            return _fileStream.ReadAsync(buffer, offset, byteCountToRead, cancellationToken);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int byteCountToWrite = count;
            if (_fileStream.Position + count + offset > _end + 1)
            {
                byteCountToWrite = (int)(_end - _fileStream.Position - offset) + 1;
            }
            return _fileStream.WriteAsync(buffer,offset,byteCountToWrite,cancellationToken);
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _fileStream.FlushAsync();
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            bufferSize = 0; // reuse same field for high water mark to avoid needing another field in the state machine
            try
            {
                while (true)
                {
                    int readBufferCount = buffer.Length;
                    if (_end - _fileStream.Position+1< readBufferCount)
                        readBufferCount = (int)(_end - _fileStream.Position + 1);
                    int bytesRead = await _fileStream.ReadAsync(buffer, 0, readBufferCount, cancellationToken).ConfigureAwait(false);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    if (bytesRead > bufferSize)
                    {
                        bufferSize = bytesRead;
                    }
                    await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                Array.Clear(buffer, 0, bufferSize); // clear only the most we used
                ArrayPool<byte>.Shared.Return(buffer, clearArray: false);
            }
        }

        public override void WriteByte(byte value)
        {
            _fileStream.WriteByte(value);
        }

        public override int ReadByte()
        {
            return _fileStream.ReadByte();
        }

        public override bool CanTimeout
        {
            get
            {
                return _fileStream.CanTimeout;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                return _fileStream.ReadTimeout;
            }

            set
            {
                _fileStream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return _fileStream.WriteTimeout;
            }

            set
            {
                _fileStream.WriteTimeout = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(!disposing)
            {
                _fileStream.Dispose();
            }
        }
    }
}