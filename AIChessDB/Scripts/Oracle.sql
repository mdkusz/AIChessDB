CREATE TABLESPACE tbs_chess_pgn
  DATAFILE 'tbs_chess_pgn.dat'
    SIZE 50M
    AUTOEXTEND ON;
    
CREATE TEMPORARY TABLESPACE tbs_temp_chess_pgn
  TEMPFILE 'tbs_temp_chess_pgn.dbf'
    SIZE 5M
    AUTOEXTEND ON;
    
alter session set "_ORACLE_SCRIPT"=true;

CREATE USER chess_pgn
  IDENTIFIED BY chesspgn
  DEFAULT TABLESPACE tbs_chess_pgn
  TEMPORARY TABLESPACE tbs_temp_chess_pgn
  QUOTA UNLIMITED on tbs_chess_pgn;
  
GRANT create session TO chess_pgn;

GRANT create table TO chess_pgn;

GRANT create view TO chess_pgn;

GRANT create any trigger TO chess_pgn;

GRANT create any procedure TO chess_pgn;

GRANT create sequence TO chess_pgn;

GRANT create synonym TO chess_pgn;

grant create job to chess_pgn;