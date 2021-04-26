import axios from 'axios';
import { useEffect, useState } from 'react';
import { Alert, Button, Col, Container, Row, Spinner, Form } from 'react-bootstrap';
import {
  BrowserRouter as Router,
  Route, Switch,
  useParams
} from "react-router-dom";
import './App.css';


function App() {


  return (
    <Router>
      <div>
        <Switch>
          <Route path="/pesquisa/:productId/:npsId">
            <Nps />
          </Route>
          <Route path="/">
            <NotFound />
          </Route>
        </Switch>
      </div>
    </Router>
  );
}

export default App;

function NotFound() {
  return (
    <div className="App">
      <header className="App-header">
        <p>
          Não encontrado
        </p>
      </header>
    </div>
  );
}

function Nps() {
  const [nps, setNps] = useState(0);
  const [comentario, setComentario] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(false);
  const [state, setState] = useState({
    id: undefined,
    titulo: undefined,
    imagem: undefined,
    descricaoLonga: undefined,
    respondido: false
  });

  const { productId, npsId } = useParams();

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const result = await axios.get(
          `${process.env.REACT_APP_API_ENDPOINT}/api/recuperar-nps/produto/${productId}/nps/${npsId}`,
        );
        setState(result.data);
        setLoading(false);
        setError(false);
      }
      catch (e) {
        console.log(e);
        setLoading(false);
        setError(true);
      }
    };

    fetchData();
  }, [productId, npsId]);

  const range = [
    1, 2, 3, 4, 5, 6, 7, 8, 9, 10
  ]
  const onClick = (value) => {
    setNps(value);
  }

  const confirmOnClick = async () => {
    try {
      setLoading(true);
      await axios.put(
        `${process.env.REACT_APP_API_ENDPOINT}/api/salvar-nps/produto/${productId}/nps/${npsId}`,
        { nota: nps, comentario: comentario }
      );
      setLoading(false);
      setError(false);
      setState({ ...state, respondido: true })
    }
    catch (e) {
      console.log(e);
      setLoading(false);
      setError(true);
    }
  };

  const comentarioOnChange = (event) => {
    setComentario(event.target.value)
  }

  return (
    <div className="App">
      {error && !loading && <Alert variant={"danger"}>
        Ocorreu um erro, tente novamente!
  </Alert>}
      {!error && loading && <Spinner animation="grow" />}
      {!error && !loading && state && state.id !== undefined && (
        <>
          <Container fluid>
            <header className="App-header">
              {!state.respondido ? (<>
                <img src={state.imagem} className="App-logo" alt="logo" />
                <p>
                  {state.titulo}
                </p>
                <div
                  dangerouslySetInnerHTML={{
                    __html: state.descricaoLonga
                  }}></div></>) : (
                <p>
                  Obrigado pela participação!
                </p>
              )}
            </header>
            {!state.respondido &&
              (
                <>
                  <Row>
                    {range.map(i => {
                      return (
                        <Col>
                          <Button variant={i === nps ? "success" : "outline-info"} onClick={() => { onClick(i) }} disabled={loading}>{i}</Button>
                        </Col>
                      );
                    })}
                  </Row>
                  <br />
                  {nps > 0 && (
                    <Row>
                      <Col>
                        <Form>
                          <Form.Group controlId="comentario">
                            <Form.Label>Comentário adicional</Form.Label>
                            <Form.Control as="textarea" rows={3} onChange={comentarioOnChange} disabled={loading} />
                          </Form.Group>
                          <Button variant="primary" type="Button" onClick={confirmOnClick} disabled={loading || nps === 0}>
                            Confirmar e Enviar
                        </Button>
                        </Form>
                      </Col>
                    </Row>
                  )}
                </>
              )
            }
          </Container>
        </>
      )}
    </div>
  );
}

