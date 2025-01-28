CREATE DATABASE arcalite CHARACTER SET utf8;
USE arcalite;
/*
CREATE TABLE profile (
  id INT(11) PRIMARY KEY AUTO_INCREMENT,
  username VARCHAR(20),
  password VARCHAR(100),
  played TIME DEFAULT 0,
  email VARCHAR(70)
);

CREATE TABLE achievement (
  id INT(11) PRIMARY KEY AUTO_INCREMENT
);

CREATE TABLE proach (
  profileid INT(11),
  achievementid INT(11),
  CONSTRAINT pk_proach PRIMARY KEY (profileid, achievementid),
  CONSTRAINT fk_proach_profile FOREIGN KEY (profileid) REFERENCES profile(id),
  CONSTRAINT fk_proach_achievement FOREIGN KEY (achievementid) REFERENCES achievement(id)
);

CREATE TABLE avatar (
  id INT(11) PRIMARY KEY AUTO_INCREMENT,
  image VARCHAR(15)
);

CREATE TABLE level (
  id INT(11) PRIMARY KEY AUTO_INCREMENT,
  image VARCHAR(15)
);

CREATE TABLE player (
  id INT(11) PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(20),
  hp INT(11),
  avatarid INT(11),
  levelid INT(11),
  playtime TIME DEFAULT 0,
  CONSTRAINT fk_player_avatar FOREIGN KEY (avatarid) REFERENCES avatar(id),
  CONSTRAINT fk_player_level FOREIGN KEY (levelid) REFERENCES level(id)
);

CREATE TABLE proplay (
  profileid INT(11),
  playerid INT(11),
  CONSTRAINT pk_proplay PRIMARY KEY (profileid, playerid),
  CONSTRAINT fk_proplay_profile FOREIGN KEY (profileid) REFERENCES profile(id),
  CONSTRAINT fk_proplay_player FOREIGN KEY (playerid) REFERENCES player(id)
);

CREATE TABLE enemy (
  id INT(11) PRIMARY KEY AUTO_INCREMENT,
  hp INT(11),
  image VARCHAR(15)
);

CREATE TABLE enemplay (
  enemyid INT(11),
  playerid INT(11),
  CONSTRAINT pk_enemplay PRIMARY KEY (enemyid, playerid),
  CONSTRAINT fk_enemplay_enemy FOREIGN KEY (enemyid) REFERENCES enemy(id),
  CONSTRAINT fk_enemplay_player FOREIGN KEY (playerid) REFERENCES player(id)
);

CREATE TABLE item (
  id INT(11) PRIMARY KEY AUTO_INCREMENT,
  image VARCHAR(15)
);

CREATE TABLE itemplay (
  itemid INT(11),
  playerid INT(11),
  CONSTRAINT pk_itemplay PRIMARY KEY (itemid, playerid),
  CONSTRAINT fk_itemplay_item FOREIGN KEY (itemid) REFERENCES item(id),
  CONSTRAINT fk_itemplay_player FOREIGN KEY (playerid) REFERENCES player(id)
);

CREATE TABLE descriptions (
  tablename VARCHAR(16),
  id INT(11),
  language VARCHAR(64),
  name VARCHAR(255),
  descr VARCHAR(4096),
  CONSTRAINT pk_descr PRIMARY KEY (tablename, id, language),
  CONSTRAINT fk_descr_achievement FOREIGN KEY (id) REFERENCES achievement(id),
  CONSTRAINT fk_descr_avatar FOREIGN KEY (id) REFERENCES avatar(id),
  CONSTRAINT fk_descr_item FOREIGN KEY (id) REFERENCES item(id),
  CONSTRAINT fk_descr_enemy FOREIGN KEY (id) REFERENCES enemy(id)
);
*/