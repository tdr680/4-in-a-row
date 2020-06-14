
public static class Utils
{
    public static void ShuffleArray(int []arr) 
    { 
        System.Random r = new System.Random(); 
          
        // Start from the last element and 
        // swap one by one. We don't need to 
        // run for the first element  
        // that's why i > 0 
        for (int i = arr.Length - 1; i > 0; i--)  
        {
            // Pick a random index 
            // from 0 to i 
            int j = r.Next(0, i+1); 
              
            // Swap arr[i] with the 
            // element at random index 
            int temp = arr[i]; 
            arr[i] = arr[j]; 
            arr[j] = temp; 
        } 
    }
}
