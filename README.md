# MySpot

## Testing notes
- Integration tests use the in-memory data in `ReservationsService`.
- Add integration tests for domain validation errors (for example invalid license plate length) once exception handling middleware is introduced.
- When persistence is added, plan to run integration tests against a real database (for example via a Docker container).

## Pre-commit
Install and enable the hooks:
```powershell
python -m pip install pre-commit
pre-commit install
```

Run hooks manually:
```powershell
pre-commit run --all-files
```

## Conventional commits: scopes
Suggested scopes for this repo:
- api
- commands
- controllers
- docs
- dto
- entities
- exceptions
- infrastructure
- parking-spots
- reservations
- serialization
- services
- tests-integration
- tests-unit
- value-objects

When proposing or creating commit messages, pick a scope from the list. If none fits, propose a new scope.
