# Biblioteka API

Proste REST API do zarządzania biblioteką, napisane w ASP.NET Core 8 z wykorzystaniem Entity Framework Core i SQLite.

## Funkcjonalności

- Dodawanie, pobieranie, edytowanie i usuwanie danych (CRUD).
- Obsługa autorów, książek i egzemplarzy.
- Relacje między encjami.
- Walidacja danych.
- Automatyczne tworzenie bazy danych.

## Endpointy

- "/authors" 
- "/books" 
- "/copies" 

## Uruchomienie

dotnet restore
dotnet run --project BibliotekaAPI


Testy dostępne pod adresem: http://localhost:5000/

Wynik testów: 3/3 pkt
