using System.Collections.Generic;

namespace ToonBlast.Model {

	public class ResolveResult {
		public readonly Dictionary<IGridPiece, ChangeInfo> changes = new Dictionary<IGridPiece, ChangeInfo>();
	}

}