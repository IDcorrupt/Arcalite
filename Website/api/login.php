<?php

require_once "config.php";

checkValidity("GET", "email", "password");

require_once "connection.php";

$email = $_GET['email'];
$password = $_GET['password'];

$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email' AND `deletedAt` IS NULL;";
$result = $db->query($query);
$num_of_profiles = intval($result->fetch_assoc()['db']);

if ($num_of_profiles == 0) {
    ReturnMessage(401,"Nincs ehhez az e-mail címhez regisztrált fiók!");
}

$query = "SELECT id, username, email FROM `profile` WHERE `email` = '$email' AND `password` = PASSWORD('$password') AND `deletedAt` IS NULL;";
$result = $db->query($query);

if ($result->num_rows == 0) {
    ReturnMessage(401,"Hibás jelszó!");
}
else if ($result->num_rows > 1) {
    ReturnMessage(401, "Több fiók bejelentkezési adatai egyeznek!");
}

ReturnResult($result->fetch_assoc());
