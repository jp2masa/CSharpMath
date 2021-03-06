using Xunit;
using System.Linq;
namespace CSharpMath.CoreTests {
  using CSharpMath.Atom;
  using Atoms = CSharpMath.Atom.Atoms;
  public class LaTeXSettingsTests {
    [Fact]
    public void ForAsciiHandlesAllInputs() {
      for (sbyte i = -36 + 1; i != -36; i++) // Break loop at arbitrary negative value (-36)
        switch (i) {
          case var _ when i < 0:
            Assert.Throws<System.ArgumentOutOfRangeException>(
              () => LaTeXSettings.ForAscii(i)
            );
            break;
          case var _ when i <= ' ':
          case (sbyte)'\u007F':
          case (sbyte)'$':
          case (sbyte)'%':
          case (sbyte)'#':
          case (sbyte)'&':
          case (sbyte)'~':
          case (sbyte)'\'':
          case (sbyte)'^': 
          case (sbyte)'_':
          case (sbyte)'{':
          case (sbyte)'}':
          case (sbyte)'\\':
            Assert.Null(LaTeXSettings.ForAscii(i));
            break;
          default:
            Assert.NotNull(LaTeXSettings.ForAscii(i));
            break;
        }
    }
    [Fact]
    public void CommandForAtomIgnoresInnerLists() {
      var atom = new Atoms.Accent("\u0308", new MathList(new Atoms.Number("1")));
      atom.Superscript.Add(new Atoms.Number("4"));
      atom.Subscript.Add(new Atoms.Variable("x"));
      Assert.Equal("ddot", LaTeXSettings.CommandForAtom(atom));
    }
    [Fact]
    public void AtomForCommandGeneratesACopy() {
      var atom = LaTeXSettings.AtomForCommand("int");
      if (atom == null) throw new Xunit.Sdk.NotNullException();
      atom.IndexRange = Range.NotFound;
      var atom2 = LaTeXSettings.AtomForCommand("int");
      if (atom2 == null) throw new Xunit.Sdk.NotNullException();
      Assert.Equal(Range.Zero, atom2.IndexRange);
    }
  }
}
