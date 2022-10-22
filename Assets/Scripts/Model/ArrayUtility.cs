namespace ToonBlast.Model {

	public static class ArrayUtility {
		public static int[,] TransposeArray(int[,] array) {
			var transposed = new int[array.GetLength(1), array.GetLength(0)];

			for (int y = 0; y < array.GetLength(1); y++) {
				for (int x = 0; x < array.GetLength(0); x++) {
					transposed[y, x] = array[x, y];
				}        
                
			}

			return transposed;
		}
	}

}