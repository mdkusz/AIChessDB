CREATE TABLESPACE tbs_chess_pgn
  ADD DATAFILE 'tbs_chess_pgn.ibd';

create schema chess_pgn;

create user chess_pgn
identified with mysql_native_password by 'chesspgn'
PASSWORD EXPIRE NEVER;

grant all on chess_pgn.* to chess_pgn;

SET GLOBAL event_scheduler = ON;