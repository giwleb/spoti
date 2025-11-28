# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 8 console application that interacts with the Spotify API to fetch playlist data and export it to CSV files. The application uses the SpotifyAPI.Web library for API interactions and CsvHelper for CSV generation.

## Build and Run Commands

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# Clean build artifacts
dotnet clean

# Restore NuGet packages
dotnet restore
```

## Architecture

The application consists of a single-file console program that:
1. Authenticates with Spotify using Client Credentials flow (stored in User Secrets)
2. Fetches playlist items from specified playlist IDs
3. Exports track information (song name, album, artists) to CSV files

Key components:
- **Authentication**: Uses Microsoft.Extensions.Configuration.UserSecrets for secure credential storage
- **Spotify Integration**: SpotifyAPI.Web library handles all Spotify API interactions
- **CSV Export**: CsvHelper library manages CSV file generation with dynamic column count based on maximum artists per track

## Configuration

The application requires Spotify API credentials configured as User Secrets:
- `Spotify:ClientId`
- `Spotify:ClientSecret`

User secrets are managed via:
```bash
dotnet user-secrets set "Spotify:ClientId" "your-client-id"
dotnet user-secrets set "Spotify:ClientSecret" "your-client-secret"
```

## Dependencies

- SpotifyAPI.Web (7.2.1) - Spotify API client
- CsvHelper (33.0.1) - CSV file generation
- Microsoft.Extensions.Configuration.UserSecrets (8.0.1) - Secure credential storage
- System.Linq.Async (6.0.1) - Async LINQ operations