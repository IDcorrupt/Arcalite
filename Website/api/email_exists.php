<?php

require_once "config.php";

checkValidity("GET", "email");

require_once "connection.php";

$email = $_GET['email'];

$sql = "SELECT * FROM `profile` WHERE email = '$email';";
$result = $db->query($sql);


$return = array("exists" => $result->num_rows > 0);

ReturnResult($return);