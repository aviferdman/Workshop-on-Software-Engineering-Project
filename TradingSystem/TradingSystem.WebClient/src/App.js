import {useEffect} from "react";
import Routes from "./routes";
import SearchBar from "./components/searchBar.js";
import SearchAppBar from "./components/AppBar";

export function useTitle(title) {
  useEffect(() => {
    document.title = 'Ecommerce - ' + title;
  }, [title]);
}

function App() {
  return (
      <Routes></Routes>
  );
}

export default App;
