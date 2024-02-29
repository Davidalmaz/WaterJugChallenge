using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class WaterJugChallengeController : ControllerBase
{
    // Handles POST requests to find a solution for the challenge
    [HttpPost]
    public IActionResult FindSolution([FromBody] WaterBucketRequest request)
    {
        // Validation: Buckets must be greater than 0 and integers
        if (request.BucketX <= 0 || request.BucketY <= 0 ||
            !IsInteger(request.BucketX) || !IsInteger(request.BucketY))
        {
            return BadRequest("Bucket must be greater than 0 and integers.");
        }

        // Create a WaterBucketChallenge object with given bucket sizes
        WaterBucketChallenge challenge = new WaterBucketChallenge(request.BucketX, request.BucketY);

        // Find a solution to the problem for the specified amount
        List<WaterBucketChallenge.Buckets> solution = challenge.FindSolution(request.AmountWantedZ);

        // If no solution is found, return "No solution" status
        if (solution.Count == 0)
        {
            return NotFound("No solution.");
        }

        // Return the solution
        return Ok(solution);
    }

    // Checks if a double value is an integer
    private bool IsInteger(double value)
    {
        return Math.Abs(value % 1) < double.Epsilon;
    }
}

// Represents the request object for the water bucket problem
public class WaterBucketRequest
{
    public int BucketX { get; set; }
    public int BucketY { get; set; }
    public int AmountWantedZ { get; set; }
}

// Represents the logic for solving the water bucket problem
public class WaterBucketChallenge
{
    public class Buckets
    {
        public int X { get; set; } 
        public int Y { get; set; } 
        public string Explanation { get; set; } = string.Empty;

    }

    private int maxBucketX; // Max capacity of Bucket X
    private int maxBucketY; // Max capacity of Bucket Y

    // Constructor to initialize bucket sizes
    public WaterBucketChallenge(int BucketX, int BucketY)
    {
        maxBucketX = BucketX;
        maxBucketY = BucketY;
    }

    // Checks if the target amount can be reached with given buckets
    private bool CheckLimitations(Buckets buckets, int target)
    {
        return buckets.X == target || buckets.Y == target ||
               buckets.X + buckets.Y == target ||
               (target % GreatestCommonDivisor(maxBucketX, maxBucketY) == 0 &&
               target <= maxBucketX + maxBucketY);
    }

    // Finds the greatest common divisor of two integers
    private int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // Finds a solution to the problem
    public List<Buckets> FindSolution(int target)
    {
        Buckets initial = new Buckets { X = 0, Y = 0 };  // Initial state of buckets
        List<Buckets> path = new List<Buckets>();  // Current path of actions
        List<Buckets> solutionPath = new List<Buckets>();  // Path to the solution

        // If target cannot be reached, return empty solution
        if (!CheckLimitations(initial, target))
        {
            return new List<Buckets>();
        }

        // Breadth-first search for finding the solution
        Queue<List<Buckets>> queue = new Queue<List<Buckets>>();
        HashSet<string> visited = new HashSet<string>();

        path.Add(initial);
        queue.Enqueue(path);
        visited.Add($"0,0");

        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            Buckets currentState = path[path.Count - 1];

            // If target amount is reached, mark the solution and break the loop
            if (currentState.X == target || currentState.Y == target)
            {
                // Mark the last step with "SOLVED"
                currentState.Explanation += " SOLVED";
                solutionPath = new List<Buckets>(path);
                break;
            }

            // Generate next possible states and add them to the queue
            List<Buckets> nextStates = new List<Buckets>
            {
                FillJarX(new Buckets { X = currentState.X, Y = currentState.Y }),
                FillJarY(new Buckets { X = currentState.X, Y = currentState.Y }),
                EmptyJarX(new Buckets { X = currentState.X, Y = currentState.Y }),
                EmptyJarY(new Buckets { X = currentState.X, Y = currentState.Y }),
                TransferXToY(new Buckets { X = currentState.X, Y = currentState.Y }),
                TransferYToX(new Buckets { X = currentState.X, Y = currentState.Y })
            };

            foreach (var nextState in nextStates)
            {
                string stateKey = $"{nextState.X},{nextState.Y}";
                if (!visited.Contains(stateKey))
                {
                    visited.Add(stateKey);
                    List<Buckets> newPath = new List<Buckets>(path);
                    newPath.Add(nextState);
                    queue.Enqueue(newPath);
                }
            }
        }

        return solutionPath ?? new List<Buckets>();
    }

    private Buckets FillJarX(Buckets buckets)
    {
        buckets.X = maxBucketX;
        buckets.Explanation = "Fill bucket X.";
        return buckets;
    }

    private Buckets FillJarY(Buckets buckets)
    {
        buckets.Y = maxBucketY;
        buckets.Explanation = "Fill bucket Y.";
        return buckets;
    }

    private Buckets EmptyJarX(Buckets buckets)
    {
        buckets.X = 0;
        buckets.Explanation = "Empty bucket X.";
        return buckets;
    }

    private Buckets EmptyJarY(Buckets buckets)
    {
        buckets.Y = 0;
        buckets.Explanation = "Empty bucket Y.";
        return buckets;
    }

    private Buckets TransferXToY(Buckets buckets)
    {
        int spaceInY = maxBucketY - buckets.Y;
        if (buckets.X <= spaceInY)
        {
            buckets.Y += buckets.X;
            buckets.X = 0;
        }
        else
        {
            buckets.X -= spaceInY;
            buckets.Y = maxBucketY;
        }
        buckets.Explanation = "Transfer from bucket X to bucket Y.";
        return buckets;
    }

    private Buckets TransferYToX(Buckets buckets)
    {
        int spaceInX = maxBucketX - buckets.X;
        if (buckets.Y <= spaceInX)
        {
            buckets.X += buckets.Y;
            buckets.Y = 0;
        }
        else
        {
            buckets.Y -= spaceInX;
            buckets.X = maxBucketX;
        }
        buckets.Explanation = "Transfer from bucket Y to bucket X.";
        return buckets;
    }
}

