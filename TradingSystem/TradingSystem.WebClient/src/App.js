import LoginPage from "./pages/Login";
import {useEffect} from "react";
import Routes from "./routes";

export function useTitle(title) {
  useEffect(() => {
    document.title = 'Ecommerce - ' + title;
  }, []);
}

function App() {
  return (
      <Routes />
  );
}

export default App;
