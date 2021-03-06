Hexacta Labs: .NET MVC - Primera parte
======================================

# Actividades

#### Ejercicio 1: Pantallas para Películas y Géneros:
El objetivo del ejercicio es retornar todos los objetos películas de la BD y listarlos en su vista correspondiente.

##### A-	Crear el service con el método FindAll:
Dentro del proyecto Data agregar dos clases llamadas GenreService y MovieService. Cada servicio con un método GetAll retornando una lista de cada entidad:
```
public IEnumerable<Movie> GetMovies()
{
    return this.moviesContext.Movies.AsQueryable();
}
```

##### B-	Crear ViewModels:
Dentro de la carpeta Models del proyecto CapacitacionMVC.Web, crear dos clases (MovieVM y GenreVM):
```
public class MovieVM
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string Plot { get; set; }
    public string CoverLink { get; set; }
    public int? Runtime { get; set; }
    public virtual ICollection<Genre> Genres { get; set; }
    
    public MovieVM()
    {
        this.Genres = new List<Genre>();
    }
    
     public MovieVM(Movie movie)
    {
        this.Id = movie.Id;
        this.Name = movie.Name;
        this.ReleaseDate = movie.ReleaseDate;
        this.Plot = movie.Plot;
        this.CoverLink = movie.CoverLink;
        this.Runtime = movie.Runtime;
        this.Genres = movie.Genres;
    }
    
    public void AsViewModel(Movie movie)
    {
        this.Id = movie.Id;
        this.Name = movie.Name;
        this.ReleaseDate = movie.ReleaseDate;
        this.Plot = movie.Plot;
        this.CoverLink = movie.CoverLink;
        this.Runtime = movie.Runtime;
        this.Genres = movie.Genres;
    }
    
    public Movie GetEntityModel()
    {
        return new Movie    
        {
            Id = this.Id,
            Name = this.Name,
            ReleaseDate = this.ReleaseDate,
            Plot = this.Plot,
            CoverLink = this.CoverLink,
            Runtime = this.Runtime,
            Genres = this.Genres
        };
    }
}
```
Además es necesario crear dos clases view model (MoviesIndexViewModel y GenresIndexViewModel) que servirán de contenedor para los view model creados en el paso anterior:
```
public class MoviesIndexModel
{
    public List<MovieVM> Movies { get; set; }

    [Display(Name = "Buscar")]
    public string SearchText { get; set; }

    public Guid? GenreId { get; set; }
}
```

##### C-	Controller: 
Dentro de la carpeta Controllers del proyecto CapacitacionMVC.Web, crear dos clases para los controladores de las pantallas (MoviesController y GenresController).
En el método de la acción Index de cada controlador agregar las llamadas de tal forma que la vista reciba la lista de view models:

```
var viewModel = new MoviesIndexModel();
var movies = movieService.GetAll();
var moviesVMs = new List<MovieVM>();

foreach(var m in movies)
{
    var vm = new MovieVM();
    vm.AsViewModel(m);
    moviesVMs.Add(vm);
}

viewModel.Movies = moviesVMs;

return View(viewModel);
```

##### D-	Crear cshtml:
Agregar en la vista Index (tipada con el viewModel correspondiente) de cada entidad una lista que muestre las distintos registros recuperados de la base:

```
<div class="list-group">
    @foreach (var movie in Model.Movies)
    {
        @movie.Name
    }
</div>
```

#### Ejercicio 2: Completando el ejemplo anterior agreguemos la posibilidad de buscar por un nombre las películas. Realizar lo mismo para las géneros.
##### A-	Agregarle un parámetro a la acción Index llamado “filtro (string?)” (puede ser null) que reciba el valor ingresado por el usuario.
##### B-	Modificar el método GetAll de MovieService para que reciba el filtro correspondiente y realice la búsqueda de las películas:

```
public List<Movie> GetMovies(string nameFilter)
{
    var movies = this.moviesContext.Movies.AsQueryable();

    if (nameFilter != null)
    {
        movies = movies.Where(w => w.Name.ToLower().Contains(nameFilter.ToLower()));
    }

    return movies.ToList();
}
```

#### C - Agregar en el Index.cshtml el código del formulario de búsqueda

```
@using (Html.BeginForm("Index", "Movies", FormMethod.Get, new { @class = "form-inline", role = "form" }))
{
    <div class="form-group" style="min-width: 400px;">
        @Html.TextBoxFor(m => m.SearchText, new { @class = "form-control input-lg", type = "search", placeholder = Html.DisplayNameFor(m => m.SearchText), autofocus = "autofocus" })
    </div>
    <button class="btn btn-primary btn-lg text-left" type="submit">Buscar</button>
}
```

#### Ejercicio 3: Completando el ejercicio anterior se pide generar un listado de películas por género. La misma debe aparecer cuando se hace click en un género. Debe tener un estilo diferente al de géneros (agregar un layout diferente en cada caso). 
##### A-	Modificar el Index de los géneros de tal forma que el nombre sea un link:

```
<div class="list-group">
    @foreach (var genre in Model.Genres)
    {
        @Html.ActionLink(genre.Name, "Index", "Movies", new { genreId = genre.Id }, new { @class = "list-group-item" })
    }
</div>
```

##### B-	Modificar la acción Index del controlador de películas para que reciba el id de un género.
##### C-	Modificar el método GetAll de MovieService para que realice un filtro por el id del género. 

#### Ejercicio 4: Para finalizar el ejercicio agregar al mismo la posibilidad de ver los detalles de una película. Utilizar display templates para mostrar y formatear las diferentes secciones:
##### A-	Agregar nuevas propiedades a la clase Movie.
##### B-	Mostrar en la nueva vista el detalle de la película utilizando DisplayFor
##### C-	Modificar la vista Index de la carpeta Movie de forma tal que cada película muestre un link llamado “Detalle” que llame a la acción Details del controlador para mostrar al detalle de la misma (se pasa el Id como parámetro).
##### D-	Agregar a MovieService un nuevo método GetById que recupere una película a partir de su Id:

```
 public Movie GetById(Guid id)
{
    return this.moviesContext.Movies.FirstOrDefault(f => f.Id == id);
}
```

##### E-	Agregar una nueva acción en el controlador de películas llamada Details que reciba el Id de una película y muestre su detalle:

```
 public ActionResult Details(Guid id)
{
    var movie = this.movieService.GetById(id);

    return this.View(new MoviesDetailsModel { Movie = movie });
}
```

##### F-	Agregar una nueva vista en la carpeta Movies llamada Details (vista tipada con el modelo MoviesDetailsModel)




