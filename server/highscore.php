<?php

$method = $_SERVER['REQUEST_METHOD'];

$response = "";
switch ($method) {
  case 'GET':
    $response = getResponse();
    break;
  case 'POST':
    $response = postResponse();
    break;
  default:
    $response = errorResponse();
    http_response_code(500);
    break;
}

echo $response;

function getResponse() {
  http_response_code(200);
  return '{ "highscore": [{"name": "Robert", "score": 50}, {"name": "Alex", "score": 40} ] }';
}

function postResponse() {
  http_response_code(200);
  
}

function errorResponse() {
  http_response_code(500);
  
}

?>