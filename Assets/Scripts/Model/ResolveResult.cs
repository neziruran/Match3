using System.Collections.Generic;

namespace Match3.Model {

	public class ResolveResult {
		public readonly Dictionary<IGridPiece, ChangeInfo> changes = new Dictionary<IGridPiece, ChangeInfo>();
	}

}