<?php

require_once "config.php";

checkValidity("GET", "type", "langid");

$langid = $_GET['langid'];

switch ($_GET["type"]) {
    case "profile":
        //limit is considerable here
        $sql = "SELECT
                    profile.username AS `Felhasználónév`,
                    profile.played AS `Játékidő`,
                    COUNT(proach.achievementid) AS `Elért mérföldkövek`,
                    COUNT(CASE WHEN player.levelid = (SELECT MAX(id) FROM level) THEN player.id END) AS `Végigjátszások`
                FROM 
                    profile 
                    LEFT JOIN proach ON profile.id = proach.profileid
                    LEFT JOIN player ON profile.id = player.profileid
                GROUP BY profile.id;";
        break;
    case "game":
        $sql = "SELECT
                    player.name AS `Karakter`,
                    profile.username AS `Profil`,
                    avatardesc.name AS `Ąvatár`,
                    player.playtime AS `Játékidő`,
                    player.levelid AS `Elért szint`,
                    COUNT(enemplay.enemyid) AS `Felfedezett ellenfelek`,
                    COUNT(itemplay.itemid) AS `Felfedezett tárgyak`
                FROM 
                    player
                    INNER JOIN profile ON player.profileid = profile.id
                    INNER JOIN avatar ON player.avatarid = avatar.id
                    INNER JOIN avatardesc ON avatardesc.avatarid = avatar.id
                    INNER JOIN lang ON avatardesc.languageid = lang.id
                    INNER JOIN enemplay ON enemplay.playerid = player.id
                    INNER JOIN itemplay ON itemplay.playerid = player.id
                WHERE lang = $langid;";
        break;
    default:
        break;
}

ReturnQuery($sql);