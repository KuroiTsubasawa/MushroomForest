<?php
require_once 'DbConnect.php';

$username = $_POST['username'] ?? '';
$password = $_POST['password'] ?? '';
$email = $_POST['email'] ?? '';

// Walidacja podstawowa
if (empty($username) || empty($password) || empty($email)) {
    echo json_encode(['success' => false, 'message' => 'Error: Wszystkie pola wymagane.']);
    exit;
}

if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    echo json_encode(['success' => false, 'message' => 'Nieprawidłowy email.']);
    exit;
}

// Sprawdzenie czy użytkownik istnieje
$stmt = $pdo->prepare("SELECT id FROM users WHERE username = ? OR email = ?");
$stmt->execute([$username, $email]);
if ($stmt->fetch()) {
    echo json_encode(['success' => false, 'message' => 'Użytkownik lub email już istnieje.']);
    exit;
}

// Zapis nowego użytkownika
$hashedPassword = password_hash($password, PASSWORD_DEFAULT);
$stmt = $pdo->prepare("INSERT INTO users (username, email, password_hash) VALUES (?, ?, ?)");
if ($stmt->execute([$username, $email, $hashedPassword])) {
    echo json_encode(['success' => true, 'message' => 'Rejestracja udana.']);
} else {
    echo json_encode(['success' => false, 'message' => 'Błąd rejestracji.']);
}
