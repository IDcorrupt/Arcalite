<?php

require_once "config.php";

checkValidity("GET", "userid", "langid");

$userid = $_GET["userid"];
$langid = $_GET["langid"];

$sql = "SELECT player.id AS id, player.name AS name, player.hp AS hp, player.levelid AS level, player.playtime AS playtime, avatar.image AS image
        FROM 
            player 
            INNER JOIN avatar ON player.avatarid = avatar.id
            INNER JOIN avatardesc ON avatar.id = avatardesc.avatarid
        WHERE 
            avatardesc.languageid = $langid AND 
            profileid = $userid";

ReturnQuery($sql);