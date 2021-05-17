import {
  BrowserRouter as Router,
  Route, Switch
} from "react-router-dom";
import NotFound from './NotFound';
import NpsPage from './NpsPage';

export function NpsRoutes() {
  return (
    <Router>
      <div>
        <Switch>
          <Route path="/pesquisa/:productId/:npsId">
            <NpsPage />
          </Route>
          <Route path="/">
            <NotFound />
          </Route>
        </Switch>
      </div>
    </Router>
  );
}

export default NpsRoutes;