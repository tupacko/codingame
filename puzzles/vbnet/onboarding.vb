Module Player
' The code below will read all the game information for you.
' On each game turn, information will be available on the standard input, you will be sent:
' -> the total number of visible enemies
' -> for each enemy, its name and distance from you
' The system will wait for you to write an enemy name on the standard output.
' Once you have designated a target:
' -> the cannon will shoot
' -> the enemies will move
' -> new info will be available for you to read on the standard input.

    Sub Main ()
        
        ' game loop
        While True
            Dim count as Integer
            count = Console.ReadLine() ' The number of current enemy ships within range

            Dim cE as String
            Dim cD as Integer = 99999
            For i as Integer = 0 To count-1
                Console.Error.WriteLine("{0}", i)
                Dim inputs as String()
                Dim enemy as String ' The name of this enemy
                Dim dist as Integer ' The distance to your cannon of this enemy
                inputs = Console.ReadLine().Split(" ")
                enemy = inputs(0)
                dist = inputs(1)
                If cd > dist Then
                    cD = dist
                    cE = enemy
                End If
            Next

            ' Write an action using Console.WriteLine()
            ' To debug: Console.Error.WriteLine("Debug messages...")

            Console.WriteLine(cE) ' The name of the most threatening enemy (HotDroid is just one example)
        End While
    End Sub
End Module