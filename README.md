### What is this?
Extension methods for Tekla's Open API.


### But why?

#### The old way
```c#
ModelObjectSelector Selector = model.GetModelObjectSelector();

// The Old Way
foreach (ModelObject MO in Selector)
{
            Beam B = MO as Beam;
            if (B != null)
            {
            Solid solid = B.GetSolid();
            }
}   

```

#### The New Way:
```c#
            List<Solid> solids = Selector.GetAllObjects().ToTeklaList<Beam>().Select(b => b.GetSolid()).ToList();
```

### The Ability To Transform via Matrices:

```c#

CoordinateSystem cs1 = getCoordinateSystem1();
CoordinateSystem cs2 = getCoordinateSystem2();

// move by operation
Beam beam = beamFactory(startPointFactory(), endPointFactory(), "1"); // we need factory methods because the same points mutate
beam.Insert();
Operation.MoveObject(beam, cs1, cs2);
beam.Select(); // gray       
```

But who wants to formulate a transformation by hand via coordinate system changes in your head.

What you really want to do is apply a matrix:


```c#

Beam beam2 = beamFactory(startPointFactory(), endPointFactory(), "2"); // 
beam2.Insert();

Matrix matrix = BeamExtensions.FromObjectToObjectTransformationMatrix(cs1, cs2);
beam2.TransformByMutation(matrix);  // apply a matrix transformation
beam2.Modify();
beam2.Select(); // update memory                        
model.CommitChanges();
```

But you could apply any transformation that you wanted!


```c#
Matrix matrix = new Matrix().RotateBy(Math.PI / 2, VectorExtensions.YAxis) 		    	              
                       .ThenDisplaceBy(new Vector().ToXaxisWCS());     // curry matrix operations

beam2.TransformByMutation(matrix);  // apply a matrix transformation

```

It's easy to craft matrix operations to do what you want.

```c#
Matrix m = new Matrix().RotateBy(Math.PI / 2, VectorExtensions.YAxis) 		    	              
                       .ThenDisplaceBy(new Vector().ToXaxisWCS());     // curry matrix operations

Point origin = new Point(1,0,0).Transform(m);
```

Did you catch that? You can apply the transformation to the point, rather than the point to the matrix:

```c#
Point newPoint = new Matrix().Transform(new Point(1,0,0));

// but I prefer calling the point, rather than the matrix e.g.
origin.Transform(m);
```

Ever wanted a simple projection?

```c#
 Vector diagonal = new Vector(1, 1, 0);
 Vector xVector = new Vector(1, 0, 0);

 Vector projectionVector = diagonal.ProjectOnto(xVector);
```

.... and a whole host of other helpers, particularly if you want to use matrices to transform objects.

Further handy helpers if you want to use text strings from users, in order to create grid lines.

Enjoy!


### Documentation

#### Installation

```sh
// .net CLI
dotnet add package Tekla.ExtensionMethods
```

```sh
// PMC (Package Manager Console)
NuGet\Install-Package Tekla.ExtensionMethods 
```

### Contributions
Contributions welcome!

If required, add a test.

### Notes fo the Maintainer: Packing Instructions

* Build the solution
* Update: the assembly version in the csproj file.
* Update: 

In Powershell:

```powershell
D:\\Documents\\repositories\\TeklaProjects\\Tekla.ExtensionMethods\\Tekla.ExtensionMethods> nuget pack -Symbols -SymbolPackageFormat snupkg
```



