<?php

require_once "config.php";


if ($_SERVER["REQUEST_METHOD"] != "GET") {
    ReturnError(405, "Hiba az API-hívásban.");
}

checkProperFields("GET", "type");

switch ($_GET["type"]) {
    case "profile":
        $sql = "SELECT
                    profile.username AS `Felhasználónév`,
                    profile.played AS `Játékidő`,
                    COUNT(proach.achievementid) AS `Elért mérföldkövek`,
                    COUNT(CASE WHEN player.levelid = (SELECT MAX(id) FROM level) THEN player.id END) AS `Végigjátszások`
                FROM 
                    profile 
                    LEFT JOIN proach ON profile.id = proach.profileid
                    LEFT JOIN player ON profile.id = player.profileid
                WHERE player.levelid = (SELECT MAX(id) FROM level)
                GROUP BY profile.id;";
        break;
    case "game":
        //ez nagyon hosszú lesz
        $sql = "";
        break;
    default:
        break;
}

require_once "connection.php";