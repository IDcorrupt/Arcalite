<?php

require_once "config.php";

checkValidity("GET", "email", "password");

require_once "connection.php";

$email = $_GET['email'];
$password = $_GET['password'];


$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email';";
$result = $db->query($query);
$num_of_profiles = intval($result->fetch_assoc()['db']);

if ($num_of_profiles == 0) {
    ReturnMessage(401,"Nincs ehhez az e-mail címhez regisztrált fiók!");
}


$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email' AND `password` = PASSWORD('$password');";
$result = $db->query($query);
$num_of_profiles = intval($result->fetch_assoc()['db']);

if ($num_of_profiles == 0) {
    ReturnMessage(401,"Hibás jelszó!");
}
else if ($num_of_profiles > 1) {
    ReturnMessage(401, "Több fiók bejelentkezési adatai egyeznek!");
}


$query = "SELECT id, username, email FROM `profile` WHERE `email` = '$email' AND `password` = PASSWORD('$password');";
$result = $db->query($query);

$return = $result->fetch_assoc();

ReturnResult($return);
