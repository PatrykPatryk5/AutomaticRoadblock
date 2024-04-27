# Automatic Roadblocks

![Build](https://github.com/yoep/AutomaticRoadblock/actions/workflows/build.yml/badge.svg?branch=master)
![Version](https://img.shields.io/github/v/tag/yoep/AutomaticRoadblock?label=version)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

Automatic Roadblocks allow the dispatching of roadblocks during a pursuit based on a selected pursuit level.
When roadblocks are deployed, custom scanner audio is played as well as for the indication that the pursuit level is automatically increased.

## Features

- Automatic roadblock placement during pursuits
- Roadblock hit/bypass detection
- Roadblock cops automatically join the pursuit after a hit/bypass
- Request a roadblock during a pursuit
- Dynamic roadblocks with light sources during the evening/night
- Manual configurable roadblock placement
- Configurable traffic redirection
- Spike strips
- Junction roadblocks

## API/plugin integration

Plugins can use the [Functions](AutomaticRoadblock/API/Functions.cs) for available APIs.

## Development

### Dependencies

- NuGet
- .NET 4.8 SDK
- Rage Plugin Hook SDK
- Rage Native UI
- LSPDFR SDK 4.9+
- Make

### Getting started

To get started using this project, do the following steps.

1. Download all Nuget packages through the `restore` target

```bash
make restore
```

2. Try to compile the project 

This target compiles a debug version of the application and copies the binaries to the Build directory.

```bash
make build
```

Use the `build-release` target to create a release version of the application.

```bash
make build-release
```

3. Run the project tests (always in Debug config)

```bash
make test
```
