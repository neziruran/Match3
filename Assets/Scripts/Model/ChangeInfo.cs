namespace Match3.Model {

	public class ChangeInfo {
		
		public bool WasCreated { get; set; }
		public int CreationTime { get; set; }
		public BoardPos FromPos { get; set; }
		public BoardPos ToPos { get; set; }
		
	}

}