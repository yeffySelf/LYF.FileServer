create database fileserver;
create table if not exists file
(
    file_id varchar(32) not null primary key,
    file_name varchar(128) not null,
    file_path varchar(256) not null,
    file_md5 varchar(32) not null,
    file_ext varchar(32) not null,
    file_mimiType varchar(32) not null,
    file_downCnt bigint not null default 0,
    file_length bigint not null default 0,
    file_createtime timestamp (0) with time zone not null default now(),
    file_creator varchar(32) not null,
    file_updatetime timestamp (0) with time zone not null default now(),
    file_updator varchar(32) not null,
    file_isDel boolean not null default false
);

create table if not exists file_detail
(
    fdetail_id varchar(32) not null primary key,
    fdetail_fileid varchar(32) not null,
    fdetail_type int not null,
    fdetail_createtime timestamp (0) with time zone not null default now(),
    fdetail_creator varchar(32) not null,
    fdetail_updatetime timestamp (0) with time zone not null default now(),
    fdetail_updator varchar(32) not null
);