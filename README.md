# Water Jug Challenge

This application solves the water bucket problem using a breadth-first search algorithm implemented in C#. The problem involves finding a sequence of actions to reach a desired amount of water in one of the buckets given their capacities.

## Algorithmic Approach

The algorithm starts with an initial state where both buckets are empty. It then explores all possible actions (filling, emptying, or transferring water between buckets) and their resulting states in a breadth-first manner until the desired amount of water is reached in one of the buckets or no solution is found. The solution is represented as a sequence of actions.

## Run the project

To run the Water Jug Challenge, ensure you have [.NET Core](https://dotnet.microsoft.com/download) installed on your system and Visual Studio Community or Visual Studio Code.

1. Clone the repository:

    ```bash
    git clone https://github.com/Davidalmaz/WaterJugChallenge.git
    ```

2. Navigate to the project directory:

    ```bash
    cd WaterJugChallenge
    ```

3. Build the project:

    ```bash
    dotnet build
    ```

4. Run the application:

    ```bash
    dotnet run / Press F10 button
    ```

5. The application will start, and you can send a POST request to the `/api/WaterJugChallenge` endpoint with a JSON payload containing `BucketX`, `BucketY`, and `AmountWantedZ` fields to find a solution.

## Validation cases with API

### Request

curl -X 'POST' \
  'https://localhost:7125/api/WaterJugChallenge' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "bucketX": 10,
  "bucketY": 2,
  "amountWantedZ": 6
}'

### Response 

```json
[
  {
    "x": 0,
    "y": 0,
    "explanation": ""
  },
  {
    "x": 10,
    "y": 0,
    "explanation": "Fill bucket X."
  },
  {
    "x": 8,
    "y": 2,
    "explanation": "Transfer from bucket X to bucket Y."
  },
  {
    "x": 8,
    "y": 0,
    "explanation": "Empty bucket Y."
  },
  {
    "x": 6,
    "y": 2,
    "explanation": "Transfer from bucket X to bucket Y. SOLVED"
  }
]
```

### Request

curl -X 'POST' \
  'https://localhost:7125/api/WaterJugChallenge' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "bucketX": 2,
  "bucketY": 100,
  "amountWantedZ": 96
}'

### Response 

```json
[
  {
    "x": 0,
    "y": 0,
    "explanation": ""
  },
  {
    "x": 0,
    "y": 100,
    "explanation": "Fill bucket Y."
  },
  {
    "x": 2,
    "y": 98,
    "explanation": "Transfer from bucket Y to bucket X."
  },
  {
    "x": 0,
    "y": 98,
    "explanation": "Empty bucket X."
  },
  {
    "x": 2,
    "y": 96,
    "explanation": "Transfer from bucket Y to bucket X. SOLVED"
  }
]
```

### Request

curl -X 'POST' \
  'https://localhost:7125/api/WaterJugChallenge' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "bucketX": 2,
  "bucketY": 6,
  "amountWantedZ": 5
}'

### Response 

```json
No solution.
```
