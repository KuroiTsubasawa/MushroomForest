<?php

require_once 'DbConnect.php';

$username = $_POST['username'] ?? '';
$password = $_POST['password'] ?? '';

if (empty($username) || empty($password)) {
    echo json_encode(['success' => false, 'message' => 'Wszystkie pola wymagane.']);
    exit;
}

$stmt = $pdo->prepare("SELECT id, password_hash FROM users WHERE username = ?");
$stmt->execute([$username]);
$user = $stmt->fetch();

if ($user && password_verify($password, $user['password_hash'])) {
    echo json_encode(['success' => true, 'message' => 'Zalogowano pomyślnie.']);
} else {
    echo json_encode(['success' => false, 'message' => 'Nieprawidłowy login lub hasło.']);
}
