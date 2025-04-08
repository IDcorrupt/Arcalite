<?php

require_once "config.php";

checkValidity("GET", "userid", "langid");

$userid = $_GET["userid"];
$langid = $_GET["langid"];

<<<<<<< Updated upstream
$sql = "SELECT player.id AS id, player.name AS name, player.hp AS hp, player.mp AS mp, player.levelid AS level, player.playtime AS playtime, avatar.splash AS image, level.image AS levelimage
=======
$sql = "SELECT player.id AS id, player.name AS name, player.hp AS hp, player.mp AS mp, player.levelid AS level, player.playtime AS playtime, avatar.splash AS image
>>>>>>> Stashed changes
        FROM 
            player 
            INNER JOIN avatar ON player.avatarid = avatar.id
            INNER JOIN avatardesc ON avatar.id = avatardesc.avatarid
            INNER JOIN level ON level.id = player.levelid
        WHERE 
            avatardesc.languageid = $langid AND 
            profileid = $userid";

ReturnQuery($sql);