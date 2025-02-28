<?php

require_once "config.php";

checkValidity("POST", "userid", "username", "email");

$userid = $_POST["userid"];
$username = $_POST["username"];
$email = $_POST["email"];

$username_given = $username != "";
$email_given = $email != "";

if (!$username_given && !$email_given) {
    ReturnMessage(200, "Nem lett adat frissítve.");
}


require_once "connection.php";

$sql = "UPDATE profile SET ";
if ($username_given) { $sql .= "username = '$username'"; }
if ($username_given && $email_given) { $sql .= ", "; }
if ($email_given) { $sql .= "email = '$email'"; }
$sql .= " WHERE id = $userid";

$result = $db->query($sql);


$sql = "SELECT username, email FROM profile WHERE id = $userid";

ReturnResult($db->query($sql)->fetch_assoc());