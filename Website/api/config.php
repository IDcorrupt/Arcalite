<?php

function ReturnError($code = 500, $message = "") {
    echo $message;
    http_response_code($code);
    die();
}

function checkProperFields($method, ...$fields) {
    if (!hasProperFields($method, $fields)) {
        ReturnError(400, "Hiba az API-hívásban.");
    }
}

function hasProperFields($method, $fields) {
    switch ($method) {

        case "POST":
            if (count($_POST) != count($fields)) { return false; }
            foreach ($fields as $f) { if (!isset($_POST[$f])) { return false; } }
            return true;
            
        case "GET":
            if (count($_GET) != count($fields)) { return false; }
            foreach ($fields as $f) { if (!isset($_GET[$f])) { return false; } }
            return true;

        default:
            return false;
    }
}