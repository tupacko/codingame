import java.util.*;
import java.io.*;
import java.math.*;

/**
 * The code below will read all the game information for you.
 * On each game turn, information will be available on the standard input, you will be sent:
 * -> the total number of visible enemies
 * -> for each enemy, its name and distance from you
 * The system will wait for you to write an enemy name on the standard output.
 * Once you have designated a target:
 * -> the cannon will shoot
 * -> the enemies will move
 * -> new info will be available for you to read on the standard input.
 **/
class Player {
    public class Enemy
    {
        private String name;
        private int distance;
        
        public void SetDistance(int distance)
        {
            this.distance = distance;
        }
        
        public int GetDistance()
        {
            return this.distance;
        }
        
        public void SetName(String name)
        {
            this.name = name;
        }
        
        public String GetName()
        {
            return this.name;
        }
    }

    private Enemy ReadEnemy(Scanner in){
        String enemy = in.next(); // The name of this enemy
        int dist = in.nextInt(); // The distance to your cannon of this enemy
        
        Enemy current = new Enemy();
        current.SetName(enemy);
        current.SetDistance(dist);
        
        return current;
    }

    private void Play()
    {
        Scanner in = new Scanner(System.in);

        // game loop
        while (true) {
            int count = in.nextInt(); // The number of current enemy ships within range
            Enemy closest = new Enemy();
            closest.SetDistance(10000);
            for (int i = 0; i < count; i++) {
                Enemy current = ReadEnemy(in);
                
                if(closest.GetDistance() > current.GetDistance())
                {
                    closest = current;
                }
            }

            // Write an action using System.out.println()
            // To debug: System.err.println("Debug messages...");

            System.out.println(closest.GetName()); // The name of the most threatening enemy (HotDroid is just one example)
        }
    }

    public static void main(String args[]) {
        new Player().Play();
    }
}