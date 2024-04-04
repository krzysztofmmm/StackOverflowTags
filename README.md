# EN
# StackOverflow Tags Project

This project fetches 100 tags using the Stack Overflow API and stores them in a SQLite database. It features methods for refreshing and retrieving tags, and the entire application is containerized. Simply run `docker-compose up` to start the application, then access the OpenAPI documentation via Swagger at `localhost:8080` to perform queries.

## Features

- **Tag Fetching and Refreshing**: Dynamically fetches and updates tags from Stack Overflow.
- **Database**: Utilizes SQLite for efficient data storage.
- **Containerization**: Fully containerized with Docker for straightforward setup.
- **Documentation**: Includes OpenAPI documentation accessible through Swagger UI.

The test project is integrated into the main solution, ensuring that all functionalities are thoroughly tested.

## Getting Started

To run the application:
1. Ensure Docker is installed on your system.
2. Navigate to the project directory and run `docker-compose up`.
3. Visit `http://localhost:8080` to access the Swagger UI and API endpoints.


# PL
# Projekt Tagów StackOverflow

Projekt ten pobiera 100 tagów przy użyciu API Stack Overflow i przechowuje je w bazie danych SQLite. Oferuje metody do odświeżania i pobierania tagów, a cała aplikacja jest skonteneryzowana. Wystarczy uruchomić `docker-compose up`, aby rozpocząć działanie aplikacji, a następnie uzyskać dostęp do dokumentacji OpenAPI za pomocą Swaggera pod adresem `localhost:8080`, aby wykonywać zapytania.

## Funkcje

- **Pobieranie i odświeżanie tagów**: Dynamiczne pobieranie i aktualizowanie tagów ze Stack Overflow.
- **Baza danych**: Wykorzystanie SQLite dla efektywnego przechowywania danych.
- **Konteneryzacja**: Pełna konteneryzacja z Dockerem.
- **Dokumentacja**: Zawiera dokumentację OpenAPI dostępną przez UI Swaggera.

Projekt testowy jest zintegrowany z głównym rozwiązaniem, co zapewnia, że wszystkie funkcjonalności są dokładnie przetestowane.

## Jak uruchomić

Aby uruchomić aplikację:
1. Upewnij się, że Docker jest zainstalowany na Twoim systemie.
2. Przejdź do katalogu projektu i uruchom `docker-compose up`.
3. Odwiedź `http://localhost:8080`, aby uzyskać dostęp do UI Swaggera i endpointów API.
