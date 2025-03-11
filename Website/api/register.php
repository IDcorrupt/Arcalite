<?php

require_once "config.php";

checkValidity("POST", "username", "email", "password");

require_once "connection.php";

$username = $_POST['username'];
$password = $_POST['password'];
$email = $_POST['email'];


$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email' AND `deletedAt` IS NULL";
$result = $db->query($query);
$num_of_users = intval($result->fetch_assoc()['db']);

if ($num_of_users > 0) {
    ReturnMessage(409, "Már van fiók regisztrálva ehhez az e-mail címhez!");
}


$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `username` = '$username' AND `deletedAt` IS NULL";
$result = $db->query(query: $query);
$num_of_users = intval($result->fetch_assoc()['db']);

if ($num_of_users > 0) {
    ReturnMessage(409, "Már van fiók ilyen felhasználónévvel!");
}


$query = "INSERT INTO `profile` (`username`, `email`, `password`) VALUES ('$username','$email',PASSWORD('$password'))";
$result = $db->query($query);

if (!$result) {
    ReturnMessage(500, "Hiba az adatok feltöltése közben.");
}


$response = array(
    "code" => 201,
    "message" => "Sikeres regisztráció!"
);

ReturnResult($response, 201);