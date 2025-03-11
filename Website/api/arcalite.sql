-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 11. 09:30
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.0.30

SET FOREIGN_KEY_CHECKS=0;
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `arcalite`
--
CREATE DATABASE IF NOT EXISTS `arcalite` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `arcalite`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `achdesc`
--

DROP TABLE IF EXISTS `achdesc`;
CREATE TABLE `achdesc` (
  `achievementid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `achievement`
--

DROP TABLE IF EXISTS `achievement`;
CREATE TABLE `achievement` (
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `achievement`
--

REPLACE INTO `achievement` (`id`) VALUES
(1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `avatar`
--

DROP TABLE IF EXISTS `avatar`;
CREATE TABLE `avatar` (
  `id` int(11) NOT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `avatar`
--

REPLACE INTO `avatar` (`id`, `image`) VALUES
(1, 'kep1.png'),
(2, 'kep02.png'),
(3, 'forg.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `avatardesc`
--

DROP TABLE IF EXISTS `avatardesc`;
CREATE TABLE `avatardesc` (
  `avatarid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `avatardesc`
--

REPLACE INTO `avatardesc` (`avatarid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Első avatár', 'Alapértelmezett avatár'),
(1, 2, 'First avatar', 'Default avatar'),
(2, 1, 'Második avatár', 'Alapértelmezett avatár'),
(2, 2, 'Second avatar', 'Default avatar'),
(3, 1, 'Béak', 'Brek-brek'),
(3, 2, 'Forg', 'Ribbit-ribbit');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemplay`
--

DROP TABLE IF EXISTS `enemplay`;
CREATE TABLE `enemplay` (
  `enemyid` int(11) NOT NULL,
  `playerid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `enemplay`
--

REPLACE INTO `enemplay` (`enemyid`, `playerid`) VALUES
(1, 1),
(1, 3),
(1, 4),
(2, 1),
(2, 4),
(3, 1),
(3, 4),
(4, 1),
(4, 4),
(5, 4),
(6, 4),
(7, 4),
(8, 4),
(9, 4);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemy`
--

DROP TABLE IF EXISTS `enemy`;
CREATE TABLE `enemy` (
  `id` int(11) NOT NULL,
  `hp` int(11) DEFAULT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `enemy`
--

REPLACE INTO `enemy` (`id`, `hp`, `image`) VALUES
(1, 100, 'zombie.png'),
(2, 75, 'skeleton.png'),
(3, 150, 'troll.png'),
(4, 100, 'lich.png'),
(5, 100, 'robot.png'),
(6, 150, 'axerobot.png'),
(7, 75, 'photonrobot.png'),
(8, 150, 'knight.png'),
(9, 100, 'photonturret.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemydesc`
--

DROP TABLE IF EXISTS `enemydesc`;
CREATE TABLE `enemydesc` (
  `enemyid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `enemydesc`
--

REPLACE INTO `enemydesc` (`enemyid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Zombi', 'Alapszintű közelharci szörny. Közel jön, megüt.'),
(2, 1, 'Csontváz', 'Alapszintű mágus.'),
(3, 1, 'Troll gólem', 'Alapszintű elit közelharci szörny. Odajön, megüt. Alapképessége az átlagnál nagyobb erejű ütése, illetve különleges képessége a föld megrengetése a földre ütéssel, mely megnehezíti ellenfeleit a mozgásban.'),
(4, 1, 'Lich', 'Alapszintű elit mágus. Alaptámadásai erősebbek az átlagnál, különleges képessége, hogy segítségére csontvázakat képes teremteni.'),
(5, 1, 'Robot', 'A mechanikus világ alapszörnye. Képességei nincsenek, egyszerű közelharci ellenfél.'),
(6, 1, 'Baltás robot', 'Különleges, harcra hatékony robot. Kezei helyén balta van, mellyel egyszerűen győzi le ellenfeleit.'),
(7, 1, 'Fényágyús robot', 'A mechanikus világ mágusa. Fényágyúja nagy sebességgel szór ellenfeleire fotonokat, melyek együttesen égetik porrá a céljukat.'),
(8, 1, 'Lovag', 'Pengeéles kardjával és villámgyors lovával komoly kihívást képes teremteni ellenfeleinek. Különleges képessége a kiszemelt ellenfelére való ráfutás, mely sebességével alig lehet felvenni a versenyt.'),
(9, 1, 'Lövőtorony', 'Ez a szerkezet is a fotonokkal égetés technikáját használja, ám egy új fejlesztésű technológia által sokkal hatékonyabb, fájdalmasabb minden lövése, és minderre fel képes követőlövedékeket is kilőni, melyek biztosítják a teli találatot.');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `item`
--

DROP TABLE IF EXISTS `item`;
CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `item`
--

REPLACE INTO `item` (`id`, `image`) VALUES
(1, 'bot.png'),
(2, 'fakard.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `itemdesc`
--

DROP TABLE IF EXISTS `itemdesc`;
CREATE TABLE `itemdesc` (
  `itemid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `itemdesc`
--

REPLACE INTO `itemdesc` (`itemid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Bot', 'Egy bot, amit az felvettél az út széléről, mert nagyon megtetszett az alakja. Mondjuk azóta is jól bírja a csatározást.'),
(2, 1, 'Fakard', 'Egy fából faragott kard. Úgy gondoltad a botod nem lesz elég, ezért jobb híján egy fatuskóból álltál neki kifaragni az első kardodat.');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `itemplay`
--

DROP TABLE IF EXISTS `itemplay`;
CREATE TABLE `itemplay` (
  `itemid` int(11) NOT NULL,
  `playerid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `itemplay`
--

REPLACE INTO `itemplay` (`itemid`, `playerid`) VALUES
(1, 1),
(1, 2),
(1, 4),
(2, 4);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `lang`
--

DROP TABLE IF EXISTS `lang`;
CREATE TABLE `lang` (
  `id` int(11) NOT NULL,
  `name` varchar(64) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `lang`
--

REPLACE INTO `lang` (`id`, `name`) VALUES
(1, 'Magyar'),
(2, 'English');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `level`
--

DROP TABLE IF EXISTS `level`;
CREATE TABLE `level` (
  `id` int(11) NOT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `level`
--

REPLACE INTO `level` (`id`, `image`) VALUES
(1, 'Kep1.png'),
(2, 'Kep2.png'),
(3, 'Kep3.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `leveldesc`
--

DROP TABLE IF EXISTS `leveldesc`;
CREATE TABLE `leveldesc` (
  `levelid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `player`
--

DROP TABLE IF EXISTS `player`;
CREATE TABLE `player` (
  `id` int(11) NOT NULL,
  `name` varchar(64) DEFAULT NULL,
  `hp` int(11) DEFAULT NULL,
  `profileid` int(11) DEFAULT NULL,
  `avatarid` int(11) DEFAULT NULL,
  `levelid` int(11) DEFAULT NULL,
  `playtime` time DEFAULT '00:00:00'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `player`
--

REPLACE INTO `player` (`id`, `name`, `hp`, `profileid`, `avatarid`, `levelid`, `playtime`) VALUES
(1, 'Haró', 100, 1, 1, 1, '00:00:00'),
(2, 'Harcos', 94, 3, 1, 1, '00:00:00'),
(3, 'Toldi Miklós', 100, 3, 2, 1, '01:33:00'),
(4, 'Ferg', 126, 3, 3, 3, '03:46:00');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `proach`
--

DROP TABLE IF EXISTS `proach`;
CREATE TABLE `proach` (
  `profileid` int(11) NOT NULL,
  `achievementid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `proach`
--

REPLACE INTO `proach` (`profileid`, `achievementid`) VALUES
(3, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `profile`
--

DROP TABLE IF EXISTS `profile`;
CREATE TABLE `profile` (
  `id` int(11) NOT NULL,
  `username` varchar(64) DEFAULT NULL,
  `password` varchar(128) DEFAULT NULL,
  `played` time DEFAULT '00:00:00',
  `email` varchar(128) DEFAULT NULL,
  `deletedAt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `profile`
--

REPLACE INTO `profile` (`id`, `username`, `password`, `played`, `email`, `deletedAt`) VALUES
(1, 'haro', '*B70449F7A45FD795790A34AB04C111FADA2ABA77', '00:00:00', 'haro@gmail.com', NULL),
(2, 'feri', '*AC41464BE9A0402F6C67C07B317D773A5087E366', '00:00:00', 'ferenc001_@gmail.com', NULL),
(3, 'Laci', '*A7395F5F1F5F50654D965778F1FA7C6702350C97', '05:19:00', 'zeczi.laszlo@gmail.com', NULL),
(4, 'zoli', '*F2DE9CCD4C692570BBEB3218207B889F3204635C', '01:22:00', 'zoli@gmail.com', NULL),
(5, 'leheldani', '*C2B970DB815A941E8CA127C0CF4C83BDC82DA9B6', '00:24:00', 'ramszi@gmail.com', NULL);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `achdesc`
--
ALTER TABLE `achdesc`
  ADD PRIMARY KEY (`achievementid`,`languageid`),
  ADD KEY `fk_achdesc_languages` (`languageid`);

--
-- A tábla indexei `achievement`
--
ALTER TABLE `achievement`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `avatar`
--
ALTER TABLE `avatar`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `avatardesc`
--
ALTER TABLE `avatardesc`
  ADD PRIMARY KEY (`avatarid`,`languageid`),
  ADD KEY `fk_avatardesc_languages` (`languageid`);

--
-- A tábla indexei `enemplay`
--
ALTER TABLE `enemplay`
  ADD PRIMARY KEY (`enemyid`,`playerid`),
  ADD KEY `fk_enemplay_player` (`playerid`);

--
-- A tábla indexei `enemy`
--
ALTER TABLE `enemy`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `enemydesc`
--
ALTER TABLE `enemydesc`
  ADD PRIMARY KEY (`enemyid`,`languageid`),
  ADD KEY `fk_enemydesc_languages` (`languageid`);

--
-- A tábla indexei `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `itemdesc`
--
ALTER TABLE `itemdesc`
  ADD PRIMARY KEY (`itemid`,`languageid`),
  ADD KEY `fk_itemdesc_languages` (`languageid`);

--
-- A tábla indexei `itemplay`
--
ALTER TABLE `itemplay`
  ADD PRIMARY KEY (`itemid`,`playerid`),
  ADD KEY `fk_itemplay_player` (`playerid`);

--
-- A tábla indexei `lang`
--
ALTER TABLE `lang`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `level`
--
ALTER TABLE `level`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `leveldesc`
--
ALTER TABLE `leveldesc`
  ADD PRIMARY KEY (`levelid`,`languageid`),
  ADD KEY `fk_leveldesc_languages` (`languageid`);

--
-- A tábla indexei `player`
--
ALTER TABLE `player`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_player_avatar` (`avatarid`),
  ADD KEY `fk_player_level` (`levelid`),
  ADD KEY `fk_player_profile` (`profileid`);

--
-- A tábla indexei `proach`
--
ALTER TABLE `proach`
  ADD PRIMARY KEY (`profileid`,`achievementid`),
  ADD KEY `fk_proach_achievement` (`achievementid`);

--
-- A tábla indexei `profile`
--
ALTER TABLE `profile`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `achievement`
--
ALTER TABLE `achievement`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT a táblához `avatar`
--
ALTER TABLE `avatar`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `enemy`
--
ALTER TABLE `enemy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT a táblához `item`
--
ALTER TABLE `item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `lang`
--
ALTER TABLE `lang`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `level`
--
ALTER TABLE `level`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `player`
--
ALTER TABLE `player`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `profile`
--
ALTER TABLE `profile`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `achdesc`
--
ALTER TABLE `achdesc`
  ADD CONSTRAINT `fk_achdesc_achievement` FOREIGN KEY (`achievementid`) REFERENCES `achievement` (`id`),
  ADD CONSTRAINT `fk_achdesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `avatardesc`
--
ALTER TABLE `avatardesc`
  ADD CONSTRAINT `fk_avatardesc_avatar` FOREIGN KEY (`avatarid`) REFERENCES `avatar` (`id`),
  ADD CONSTRAINT `fk_avatardesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `enemplay`
--
ALTER TABLE `enemplay`
  ADD CONSTRAINT `fk_enemplay_enemy` FOREIGN KEY (`enemyid`) REFERENCES `enemy` (`id`),
  ADD CONSTRAINT `fk_enemplay_player` FOREIGN KEY (`playerid`) REFERENCES `player` (`id`);

--
-- Megkötések a táblához `enemydesc`
--
ALTER TABLE `enemydesc`
  ADD CONSTRAINT `fk_enemydesc_item` FOREIGN KEY (`enemyid`) REFERENCES `enemy` (`id`),
  ADD CONSTRAINT `fk_enemydesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `itemdesc`
--
ALTER TABLE `itemdesc`
  ADD CONSTRAINT `fk_itemdesc_item` FOREIGN KEY (`itemid`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `fk_itemdesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `itemplay`
--
ALTER TABLE `itemplay`
  ADD CONSTRAINT `fk_itemplay_item` FOREIGN KEY (`itemid`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `fk_itemplay_player` FOREIGN KEY (`playerid`) REFERENCES `player` (`id`);

--
-- Megkötések a táblához `leveldesc`
--
ALTER TABLE `leveldesc`
  ADD CONSTRAINT `fk_leveldesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`),
  ADD CONSTRAINT `fk_leveldesc_level` FOREIGN KEY (`levelid`) REFERENCES `level` (`id`);

--
-- Megkötések a táblához `player`
--
ALTER TABLE `player`
  ADD CONSTRAINT `fk_player_avatar` FOREIGN KEY (`avatarid`) REFERENCES `avatar` (`id`),
  ADD CONSTRAINT `fk_player_level` FOREIGN KEY (`levelid`) REFERENCES `level` (`id`),
  ADD CONSTRAINT `fk_player_profile` FOREIGN KEY (`profileid`) REFERENCES `profile` (`id`);

--
-- Megkötések a táblához `proach`
--
ALTER TABLE `proach`
  ADD CONSTRAINT `fk_proach_achievement` FOREIGN KEY (`achievementid`) REFERENCES `achievement` (`id`),
  ADD CONSTRAINT `fk_proach_profile` FOREIGN KEY (`profileid`) REFERENCES `profile` (`id`);
SET FOREIGN_KEY_CHECKS=1;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
