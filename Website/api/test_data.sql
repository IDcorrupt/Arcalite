REPLACE INTO lang (id, name) VALUES 
(1, 'Magyar'), 
(2, 'English');

--ENEMIES, ITEMS, LEVELS
--TODO: AT LEAST DOING THE FIRST COUPLE LEVELS (UNDEAD)

INSERT INTO enemy (id, hp, image) VALUES 
(1, 100, 'zombie.png'),
(2, 75, 'skeleton.png'),
(3, 150, 'troll.png'),
(4, 100, 'lich.png'),
(5, 100, 'robot.png'),
(6, 150, 'axerobot.png'),
(7, 75, 'photonrobot.png'),
(8, 150, 'knight.png'),
(9, 100, 'photonturret.png');

INSERT INTO enemydesc (enemyid, languageid, name, description) VALUES 
(1, 1, "Zombi", "Alapszintű közelharci szörny. Közel jön, megüt."),
(2, 1, "Csontváz", "Alapszintű mágus."),
(3, 1, "Troll gólem", "Alapszintű elit közelharci szörny. Odajön, megüt. Alapképessége az átlagnál nagyobb erejű ütése, illetve különleges képessége a föld megrengetése a földre ütéssel, mely megnehezíti ellenfeleit a mozgásban."),
(4, 1, "Lich", "Alapszintű elit mágus. Alaptámadásai erősebbek az átlagnál, különleges képessége, hogy segítségére csontvázakat képes teremteni."),
(5, 1, "Robot", "A mechanikus világ alapszörnye. Képességei nincsenek, egyszerű közelharci ellenfél."),
(6, 1, "Baltás robot", "Különleges, harcra hatékony robot. Kezei helyén balta van, mellyel egyszerűen győzi le ellenfeleit."),
(7, 1, "Fényágyús robot", "A mechanikus világ mágusa. Fényágyúja nagy sebességgel szór ellenfeleire fotonokat, melyek együttesen égetik porrá a céljukat."),
(8, 1, "Lovag", "Pengeéles kardjával és villámgyors lovával komoly kihívást képes teremteni ellenfeleinek. Különleges képessége a kiszemelt ellenfelére való ráfutás, mely sebességével alig lehet felvenni a versenyt."),
(9, 1, "Lövőtorony", "Ez a szerkezet is a fotonokkal égetés technikáját használja, ám egy új fejlesztésű technológia által sokkal hatékonyabb, fájdalmasabb minden lövése, és minderre fel képes követőlövedékeket is kilőni, melyek biztosítják a teli találatot.");

INSERT INTO item (id image) VALUES
();

INSERT INTO itemdesc (itemid, languageid, name, description) VALUES 
();

REPLACE INTO level (id, image) VALUES
(1, 'Kep1.png'),
(2, 'Kep2.png'),
(3, 'Kep3.png');

INSERT INTO leveldesc (levelid, languageid, name, description) VALUES 
();

--PROFILES, AVATARS, CHARACTERS

REPLACE INTO profile (id, username, password, played, email) VALUES
(1, "haro", PASSWORD("haro"), '00:00', "haro@gmail.com"),
(2, "feri", PASSWORD("Ferike123"), '00:00', "ferenc001_@gmail.com"),
(3, "laci", PASSWORD("LaszloLaszlo"), '05:19', "zeczi.laszlo@gmail.com"),
(4, "alcsi", PASSWORD("aladar001"), '00:24:32', "aladar1@gmail.com"),
(5, "vinko08", PASSWORD("vince2008"), '00:13:13', "vince2008@gmail.com");

REPLACE INTO avatar (id, image) VALUES
(1, "kep1.png"),
(2, "kep1.png"),
(3, "forg.png");

REPLACE INTO avatardesc (avatarid, languageid, name, description) VALUES
(1, 1, "Első avatár", "Alapértelmezett avatár"),
(2, 1, "Második avatár", "Alapértelmezett avatár"),
(3, 1, "Béak", "Brek-brek"),
(1, 2, "First avatar", "Default avatar"),
(2, 2, "Second avatar", "Default avatar"),
(3, 2, "Forg", "Ribbit-ribbit");

REPLACE INTO player (id, name, hp, profileid, avatarid, levelid, playtime) VALUES
(1, "Haró", 100, 1, 1, 1, "00:00"),
(2, "Harcos", 100, 2, 2, 1, "00:00"),
(3, "Toldi Miklós", 100, 3, 2, 1, "01:33"),
(4, "Ferg", 100, 3, 3, 2, "03:46");

INSERT INTO enemplay (playerid, enemyid) VALUES 
();

INSERT INTO itemplay (playerid, itemid) VALUES
();

--ACHIEVEMENTS (OPTIONAL)

INSERT INTO achievement (id) VALUES ();

INSERT INTO achdesc (achievementid, languageid, name, description) VALUES
();

INSERT INTO proach (profileid, achievementid) VALUES 
();