-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Feb 11. 14:39
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4

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

--
-- A tábla adatainak kiíratása `avatar`
--

--
-- A tábla adatainak kiíratása `lang`
--

INSERT INTO `lang` (`id`, `name`) VALUES
(1, 'Magyar'),
(2, 'English');

--
-- A tábla adatainak kiíratása `player`
--

INSERT INTO `avatar` (`id`, `image`) VALUES
(1, 'kep1.png'),
(2, 'kep1.png'),
(3, 'forg.png');

--
-- A tábla adatainak kiíratása `level`
--

INSERT INTO `level` (`id`, `image`) VALUES
(1, 'Kep1.png'),
(2, 'Kep2.png'),
(3, 'Kep3.png');

--
-- A tábla adatainak kiíratása `profile`
--

INSERT INTO `profile` (`id`, `username`, `password`, `played`, `email`) VALUES
(1, 'haro', '*B70449F7A45FD795790A34AB04C111FADA2ABA77', '00:00:00', 'haro@gmail.com'),
(2, 'feri', '*AC41464BE9A0402F6C67C07B317D773A5087E366', '00:00:00', 'ferenc001_@gmail.com'),
(3, 'laci', '*A7395F5F1F5F50654D965778F1FA7C6702350C97', '05:19:00', 'zeczi.laszlo@gmail.com'),
(4, 'károly01', '*C82C509EFF064438E9328BAB4B227FF2091C5045', '00:12:22', 'karesz1@gmail.com'),
(5, 'alcsi_alcsi', '*20EA642E802CE3D565FCE172685CBBD68AA8AA4A', '00:00:00', 'alcsi2008@gmail.com');
COMMIT;


INSERT INTO `player` (`id`, `name`, `hp`, `profileid`, `avatarid`, `levelid`, `playtime`) VALUES
(1, 'Haró', 100, 1, 1, 1, '00:00:00'),
(2, 'Harcos', 100, 2, 2, 1, '00:00:00'),
(3, 'Toldi Miklós', 100, 3, 2, 1, '01:33:00'),
(4, 'Ferg', 100, 3, 3, 2, '03:46:00');





--
-- A tábla adatainak kiíratása `avatardesc`
--

INSERT INTO `avatardesc` (`avatarid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Első avatár', 'Alapértelmezett avatár'),
(1, 2, 'First avatar', 'Default avatar'),
(2, 1, 'Második avatár', 'Alapértelmezett avatár'),
(2, 2, 'Second avatar', 'Default avatar'),
(3, 1, 'Béak', 'Brek-brek'),
(3, 2, 'Forg', 'Ribbit-ribbit');


INSERT INTO `enemy` (`id`, `hp`, `image`) VALUES
(1, 100, 'zombie.png'),
(2, 80, 'skeleton.png'),
(3, 1000, 'big_zombie.png');


--
-- A tábla adatainak kiíratása `enemplay`
--

INSERT INTO `enemplay` (`enemyid`, `playerid`) VALUES
(1, 4),
(2, 4);

--
-- A tábla adatainak kiíratása `enemy`
--

--
-- A tábla adatainak kiíratása `enemydesc`
--

INSERT INTO `enemydesc` (`enemyid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Zombi', 'Élőhalott. Elég basic, de hát ilyen is kell c:'),
(2, 1, 'Csontváz', 'Egy csontváz. Lő. Mint a Májnkráftban. Elég basic, de valahol el kell kezdeni.'),
(3, 1, 'Nagy zombi', 'Ugyanaz mint a kicsi. Csak nagy.');

--
-- A tábla adatainak kiíratása `item`
--

INSERT INTO `item` (`id`, `image`) VALUES
(1, 'stick.png'),
(2, 'woodsword.png');

--
-- A tábla adatainak kiíratása `itemdesc`
--

INSERT INTO `itemdesc` (`itemid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Bot', 'Egy bot'),
(2, 1, 'Fakard', 'Egy fából megfaragott kard. Nem a legélesebb kés a fiókban, de több, mint a semmi.');

--
-- A tábla adatainak kiíratása `itemplay`
--

INSERT INTO `itemplay` (`itemid`, `playerid`) VALUES
(1, 4),
(2, 4);



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
