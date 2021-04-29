import LoginPage from "./pages/Login";
import {useEffect} from "react";

export function useTitle(title) {
  useEffect(() => {
    document.title = 'Ecommerce - ' + title;
  }, []);
}

function App() {
  return (
      <LoginPage></LoginPage>
  );
}

export default App;
