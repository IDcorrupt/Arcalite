<?php

$CONFIG = array(
    "hostname" => "localhost",
    "username" => "root",
    "password" => "",
    "database" => "arcalite"
);

$db = new mysqli($CONFIG['hostname'],$CONFIG['username'],$CONFIG['password'],$CONFIG['database']);

if ($db->connect_errno != 0) {
    throw new Exception("Sikertelen kapcsolódás az adatbázishoz.\nHibakód: $db->connect_errno ($db->connect_error)");
}

$db->set_charset("utf8");