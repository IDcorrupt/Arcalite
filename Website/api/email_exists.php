<?php

require_once "config.php";

checkValidity("GET", "email");

require_once "connection.php";

$email = $_GET['email'];

$sql = "SELECT username, email FROM `profile` WHERE email = '$email' AND `deletedAt` IS NULL;";
$result = $db->query($sql);
$data = $result->fetch_assoc();

$return = array(
    "exists" => $result->num_rows > 0,
    "username" => $data["username"] ?? null,
    "email" => $data["email"] ?? null
);

ReturnResult($return);