use chess_pgn;

/* uncomment to drop all objects and recreate them

drop table move_comments;

drop table match_keywords;

drop table match_statistics;

drop table position_statistics;

drop table keyword_values;

drop table keywords;

drop table match_moves;

drop table match_positions;

drop table matches;

drop table positions;

drop table match_events;

drop table token_usage;

*/

create table match_events(
	COD_EVENT bigint unsigned  NOT NULL primary key,
	DESCRIPTION VARCHAR(50) NOT NULL
) tablespace = tbs_chess_pgn;

insert into match_events(cod_event,description)
values(1,'EVENT_CHECK'),(2,'EVENT_CHECK_MATE')
,(4,'EVENT_MULTIPLE_CHECK'),(8,'EVENT_DISCOVERED_CHECK'),(16,'EVENT_DRAW_OFFER')
,(32,'EVENT_PAWN_PROMOTED'),(64,'EVENT_PAWN_PASSANT'),(128,'EVENT_CAPTURE'),(256,'EVENT_CASTLEK')
,(512,'EVENT_CASTLEQ'),(1024,'EVENT_PAWN1'),(2048,'EVENT_WBISHOP1'),(4096,'EVENT_KNIGHT1'),
(8192,'EVENT_ROOK1'),(16384,'EVENT_QUEEN1'),(32768,'EVENT_KING1'),(65536,'EVENT_PAWN2'),
(131072,'EVENT_WBISHOP2'),(262144,'EVENT_KNIGHT2'),(524288,'EVENT_ROOK2'),(1048576,'EVENT_QUEEN2'),
(2097152,'EVENT_MOVE'),(4194304,'EVENT_BBISHOP1'),(8388608,'EVENT_BBISHOP2'),(16777216,'EVENT_BISHOP3'),
(33554432,'EVENT_KNIGHT3'),(67108864,'EVENT_ROOK3'),(134217728,'EVENT_QUEEN3');

create table keywords(
    COD_KEYWORD bigint unsigned  NOT NULL auto_increment primary key,
    KEYWORD VARCHAR(250) NOT NULL,
	KEYWORD_TYPE VARCHAR(3) DEFAULT 'MKW',
    DESCRIPTION VARCHAR(4000),
	CONSTRAINT UK_KEYWORDS
		UNIQUE(KEYWORD,KEYWORD_TYPE)
) tablespace = tbs_chess_pgn;

create table keyword_values(
	COD_VALUE bigint unsigned  NOT NULL auto_increment primary key,
    KW_VALUE VARCHAR(500)
) tablespace = tbs_chess_pgn;

create table positions(
    cod_position bigint unsigned  NOT NULL auto_increment primary key,
	board varchar(64) not null,
	black_pawns tinyint unsigned default 0,
	white_pawns tinyint unsigned default 0,
	black_rooks tinyint unsigned default 0,
	white_rooks tinyint unsigned default 0,
	black_bishops tinyint unsigned default 0,
	white_bishops tinyint unsigned default 0,
	black_knights tinyint unsigned default 0,
	white_knights tinyint unsigned default 0,
	black_queens tinyint unsigned default 0,
	white_queens tinyint unsigned default 0,
	sts_date datetime,
	constraint UK_BOARD
		UNIQUE(BOARD)
) tablespace = tbs_chess_pgn;

create table position_statistics(
	COD_KEYWORD bigint unsigned  NOT NULL, 
	COD_POSITION bigint unsigned  NOT NULL, 
	ST_VALUE double,
    CONSTRAINT PK_POSSTS
        PRIMARY KEY(COD_KEYWORD,COD_POSITION),
    CONSTRAINT FK_PS_POSITION
        FOREIGN KEY(COD_POSITION) 
        REFERENCES positions(COD_POSITION),
    CONSTRAINT FK_PS_KEYWORD
        FOREIGN KEY(COD_KEYWORD) 
        REFERENCES keywords(COD_KEYWORD)
) tablespace = tbs_chess_pgn;

create table matches(
    COD_MATCH bigint unsigned  NOT NULL auto_increment primary key,
	INITIAL_POSITION bigint unsigned  NOT NULL,
	MATCH_DESCRIPTION VARCHAR(1000),
	MATCH_DATE VARCHAR(30),
	WHITE VARCHAR(100),
	BLACK VARCHAR(100),
    RESULT tinyint unsigned  NOT NULL,
	RESULT_TEXT VARCHAR(8) NOT NULL,
	MOVE_COUNT smallint unsigned NOT NULL,
	FULLMOVE_COUNT smallint unsigned NOT NULL,
	sts_date datetime,
	creation_date datetime,
	sha256 VARBINARY(32),
    CONSTRAINT FK_INITIAL_POSITION
        FOREIGN KEY(INITIAL_POSITION) 
        REFERENCES positions(COD_POSITION)
) tablespace = tbs_chess_pgn;

create table match_statistics(
	COD_KEYWORD bigint unsigned  NOT NULL,
	COD_MATCH bigint unsigned  NOT NULL,
	ST_VALUE DOUBLE,
    CONSTRAINT PK_MATCHSTS
        PRIMARY KEY(COD_KEYWORD,COD_MATCH),
    CONSTRAINT FK_MS_MATCH
        FOREIGN KEY(COD_MATCH) 
        REFERENCES matches(COD_MATCH),
    CONSTRAINT FK_MS_KEYWORD
        FOREIGN KEY(COD_KEYWORD) 
        REFERENCES keywords(COD_KEYWORD)
) tablespace = tbs_chess_pgn;

create table match_positions (
    COD_MATCH bigint unsigned  NOT NULL,
    COD_POSITION bigint unsigned  NOT NULL,
    POSITION_ORDER int unsigned  NOT NULL,
	POSITION_EVENTS bigint unsigned  default 0,
    SCORE double,
	sts_date datetime,
    CONSTRAINT PK_MATCH_POSITIONS
        PRIMARY KEY(COD_MATCH,COD_POSITION,POSITION_ORDER),
    CONSTRAINT FK_MP_POSITION
        FOREIGN KEY(COD_POSITION) 
        REFERENCES positions(COD_POSITION),
    CONSTRAINT FK_MP_MATCH
        FOREIGN KEY(COD_MATCH) 
        REFERENCES matches(COD_MATCH)        
) tablespace = tbs_chess_pgn;

create table match_moves(
	COD_MATCH bigint unsigned  NOT NULL,
    FROM_POSITION bigint unsigned  NOT NULL,
	TO_POSITION bigint unsigned  NOT NULL,
	MOVE_ORDER int unsigned  NOT NULL,
	MOVE_NUMBER int unsigned  NOT NULL,
	MOVE_PLAYER tinyint unsigned  NOT NULL,
	MOVE_EVENTS bigint unsigned  default 0,
	MOVE_FROM tinyint unsigned  NOT NULL,
	MOVE_TO tinyint unsigned  NOT NULL,
	MOVE_AN_TEXT VARCHAR(10),
	COMMENTS tinyint unsigned  default 0,
    SCORE double,
	CONSTRAINT PK_MATCH_MOVES
		PRIMARY KEY(COD_MATCH,MOVE_ORDER),
	CONSTRAINT UK_MOVEPLAYER
		UNIQUE(COD_MATCH,MOVE_NUMBER,MOVE_PLAYER),
    CONSTRAINT FK_FROM_POSITION
        FOREIGN KEY(FROM_POSITION) 
        REFERENCES positions(COD_POSITION),
    CONSTRAINT FK_MMT_POSITION
        FOREIGN KEY(COD_MATCH,TO_POSITION,MOVE_ORDER) 
        REFERENCES match_positions(COD_MATCH,COD_POSITION,POSITION_ORDER),
    CONSTRAINT FK_MM_MATCH
        FOREIGN KEY(COD_MATCH) 
        REFERENCES matches(COD_MATCH)
) tablespace = tbs_chess_pgn;

create table match_keywords(
    COD_KEYWORD bigint unsigned  NOT NULL,
	COD_VALUE bigint unsigned  NOT NULL,
	COD_MATCH bigint unsigned  NOT NULL,
    CONSTRAINT PK_MKEYWORDS
        PRIMARY KEY(COD_KEYWORD, COD_MATCH),
	CONSTRAINT FK_MKV_KEYWORD
		FOREIGN KEY(COD_KEYWORD)
		REFERENCES KEYWORDS(COD_KEYWORD),
	CONSTRAINT FK_MKV_VALUE
		FOREIGN KEY(COD_VALUE)
		REFERENCES KEYWORD_VALUES(COD_VALUE),
	CONSTRAINT FK_MKV_MATCH
		FOREIGN KEY(COD_MATCH)
		REFERENCES MATCHES(COD_MATCH)
) tablespace = tbs_chess_pgn;

create table move_comments(
    COD_COMMENT bigint unsigned  NOT NULL auto_increment primary key,
    COD_MATCH bigint unsigned  NOT NULL,
	MOVE_ORDER int unsigned  NOT NULL ,
    COMMENT_TEXT VARCHAR(4000) NOT NULL,
	COMMENT_ORDER smallint unsigned  NOT NULL ,
    CONSTRAINT FK_MOVE_COMMENT
        FOREIGN KEY(COD_MATCH,MOVE_ORDER) 
        REFERENCES match_moves(COD_MATCH,MOVE_ORDER)
) tablespace = tbs_chess_pgn;

create index IX_MATCHMOVECOUNT on matches(move_count);

create index IX_POSPZCOUNT on positions(white_queens,black_queens,white_rooks,black_rooks,white_bishops,black_bishops,white_knights,black_knights,white_pawns,black_pawns);

create index IX_POSITIONSTATS on positions(white_queens,sts_date);

create index IX_KEYVALUE on keyword_values(cod_value,kw_value);

CREATE INDEX ix_matches_sha256 ON matches(sha256);

commit;

create or replace view VW_MATCH_POSITIONS AS
select m.COD_MATCH,m.INITIAL_POSITION,pi.board as initial_board,pi.black_pawns as initial_black_pawns,pi.white_pawns as initial_white_pawns,pi.black_rooks as initial_black_rooks,
pi.white_rooks as initial_white_rooks,pi.black_bishops as initial_black_bishops,pi.white_bishops as initial_white_bishops,pi.black_knights as initial_black_knights,
pi.white_knights as initial_white_knights,pi.black_queens as initial_black_queens,pi.white_queens as initial_white_queens,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,
m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mp.POSITION_ORDER,mp.POSITION_EVENTS,mp.SCORE,mpn.board,mpn.black_pawns,mpn.white_pawns,mpn.black_rooks,
mpn.white_rooks,mpn.black_bishops,mpn.white_bishops,mpn.black_knights,mpn.white_knights,mpn.black_queens,mpn.white_queens
from matches m join positions pi on m.initial_position = pi.cod_position
join match_positions mp on mp.cod_match = m.cod_match join positions mpn on mp.cod_position = mpn.cod_position;

create or replace view VW_MATCH_MOVES AS
select m.COD_MATCH,m.INITIAL_POSITION,pi.board as initial_board,pi.black_pawns as initial_black_pawns,pi.white_pawns as initial_white_pawns,pi.black_rooks as initial_black_rooks,
pi.white_rooks as initial_white_rooks,pi.black_bishops as initial_black_bishops,pi.white_bishops as initial_white_bishops,pi.black_knights as initial_black_knights,
pi.white_knights as initial_white_knights,pi.black_queens as initial_black_queens,pi.white_queens as initial_white_queens,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,
m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mm.MOVE_ORDER,mm.MOVE_NUMBER,mm.MOVE_PLAYER,mm.MOVE_EVENTS,mm.MOVE_AN_TEXT,mm.COMMENTS,mm.SCORE,mm.MOVE_FROM,mm.FROM_POSITION,fp.board as from_board,fp.black_pawns as from_black_pawns,fp.white_pawns as from_white_pawns,fp.black_rooks as from_black_rooks,
fp.white_rooks as from_white_rooks,fp.black_bishops as from_black_bishops,fp.white_bishops as from_white_bishops,fp.black_knights as from_black_knights,fp.white_knights as from_white_knights,fp.black_queens as from_black_queens,fp.white_queens as from_white_queens,
mm.MOVE_TO,mm.TO_POSITION,tp.board as to_board,tp.black_pawns as to_black_pawns,tp.white_pawns as to_white_pawns,tp.black_rooks as to_black_rooks,
tp.white_rooks as to_white_rooks,tp.black_bishops as to_black_bishops,tp.white_bishops as to_white_bishops,tp.black_knights as to_black_knights,tp.white_knights as to_white_knights,tp.black_queens as to_black_queens,tp.white_queens as to_white_queens
from matches m join positions pi on m.initial_position = pi.cod_position
join match_moves mm on mm.cod_match = m.cod_match join positions fp on mm.from_position = fp.cod_position join positions tp on mm.to_position = tp.cod_position;

create or replace view VW_MATCH_KEYWORDS as
select m.COD_MATCH,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mk.COD_KEYWORD,k.KEYWORD,kv.kw_value,ks.KEYWORD statistic,ms.st_value 
from matches m join match_keywords mk on mk.COD_MATCH = m.COD_MATCH
join keywords k on k.COD_KEYWORD = mk.COD_KEYWORD
join keyword_values kv on kv.COD_VALUE = mk.COD_VALUE
join match_statistics ms on ms.COD_MATCH = m.COD_MATCH
join keywords ks on ks.COD_KEYWORD = ms.COD_KEYWORD;

create table token_usage(
    COD_USAGE bigint unsigned  NOT NULL auto_increment primary key,
    USAGE_DATE datetime not null,
	PLAYER_NAME varchar(50) not null,
	MODEL_NAME VARCHAR(50) NOT NULL,
    TEMPERATURE DECIMAL(3,2),
	TOP_P DECIMAL(3,2),
	INPUT_TOKENS int unsigned default 0,
	OUTPUT_TOKENS int unsigned default 0,
	AUDIO_INPUT_TOKENS int unsigned default 0,
	AUDIO_OUTPUT_TOKENS int unsigned default 0,
	REASONING_TOKENS int unsigned default 0,
	INPUT_CACHED_TOKENS int unsigned default 0,
	INPUT_IMAGE_TOKENS int unsigned default 0,
	INPUT_TEXT_TOKENS int unsigned default 0
) tablespace = tbs_chess_pgn;
