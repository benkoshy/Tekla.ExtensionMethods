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
                Beam beam1_moved_by_operation = beamFactory(startPointFactory(), endPointFactory(), "1"); // we need factory methods because the same points mutate
                beam1_moved_by_operation.Insert();
                Operation.MoveObject(beam1_moved_by_operation, cs1, cs2);
                beam1_moved_by_operation.Select(); // gray       

                // set up second beamGeometric
                Beam beam2manuallyTransformed = beamFactory(startPointFactory(), endPointFactory(), "2"); // orange class string            
                beam2manuallyTransformed.Insert();

                Matrix final = BeamExtensions.FromObjectToObjectTransformationMatrix(cs1, cs2);
                beam2manuallyTransformed.TransformByMutation(final);
                beam2manuallyTransformed.Modify();
                beam2manuallyTransformed.Select(); // update memory                        
                model.CommitChanges();
```

But you could apply any transformation that you wanted!


### Matrix Extensions

```c#
Matrix m = new Matrix().RotateBy(Math.PI / 2, VectorExtensions.YAxis  ) 		    	              .ThenDisplaceBy(new Vector().ToXaxisWCS());

            Point origin = new Point(1,0,0).Transform(m);
```

Ever wanted a simple projection?

You don’t have to hand craft them anymore:

```c#
 Vector diagonal = new Vector(1, 1, 0);

 Vector xVector = new Vector(1, 0, 0);

 Vector projectionVector = diagonal.ProjectOnto(xVector);

```


… and a whole host of other helpers, particularly if you want to use matrices to transform objects.

Further handy helpers if you want to use text strings from users, in order to create grid lines.


Enjoy!



### Warning: Under Development
The API is under rapid development and is released as an alpha version.

Breaking changes will be made as the API iterated upon.

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

### Maintainer - Packing Instructions

In Powershell:

```powershell
D:\\Documents\\repositories\\TeklaProjects\\Tekla.ExtensionMethods\\Tekla.ExtensionMethods> nuget pack -Symbols -SymbolPackageFormat snupkg
```



