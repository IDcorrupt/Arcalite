<?php

require_once "config.php";

checkValidity("GET", "userid", "langid");

$userid = $_GET["userid"];
$langid = $_GET["langid"];

$sql = "SELECT player.id AS id, player.name AS name, player.hp AS hp, player.mp AS mp, player.levelid AS level, player.playtime AS playtime, avatar.image AS image
        FROM 
            player 
            INNER JOIN profile ON player.profileid = profile.id
            INNER JOIN avatar ON player.avatarid = avatar.id
            INNER JOIN avatardesc ON avatar.id = avatardesc.avatarid
        WHERE 
            avatardesc.languageid = $langid AND 
            profile.id = $userid";

ReturnQuery($sql);