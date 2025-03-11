<?php

require_once "config.php";

checkValidity("GET", "type", "langid");

$langid = $_GET['langid'];

$sql="SELECT avatar.id AS id, avatar.image AS image, avatardesc.name AS name, avatardesc.description AS description
        FROM avatar INNER JOIN avatardesc ON avatar.id = avatardesc.avatarid
        WHERE avatardesc.languageid=$langid";

ReturnQuery($sql);