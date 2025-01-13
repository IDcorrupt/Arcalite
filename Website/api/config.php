<?php

function ReturnError($code = 500, $message = "") {
    echo $message;
    http_response_code($code);
    die();
}