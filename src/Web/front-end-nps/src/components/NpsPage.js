import axios from 'axios';
import { useEffect, useState } from 'react';
import { Alert, Button, Col, Container, Form, Row, Spinner } from 'react-bootstrap';
import {
  useParams
} from "react-router-dom";

function NpsPage() {
  const [nps, setNps] = useState(null);
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
          `${process.env.REACT_APP_API_ENDPOINT}/api/recuperar-nps/${productId}/${npsId}`,
          { headers: { 'x-access-token': '5FEC3F5D-098D-47F9-BC3C-15D71E730770' } }
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
    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
  ];

  const rangeColors = [
    "danger", "danger", "danger", "danger", "danger", "danger", "danger", "warning", "warning", "success", "success"
  ];

  const onClick = (value) => {
    setNps(value);
  }

  const confirmOnClick = async () => {
    try {
      setLoading(true);
      await axios.put(
        `${process.env.REACT_APP_API_ENDPOINT}/api/salvar-nps/${productId}/${npsId}`,
        { nota: nps, comentario: comentario },
        { headers: { 'x-access-token': '5FEC3F5D-098D-47F9-BC3C-15D71E730770' } }
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
          <Container>
            <header className="App-header">
              {!state.respondido ? (<>
                <img src={state.imagem} className="App-logo" alt="logo" />
                <p>
                  <b>{state.titulo}</b>
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
                    <Col>
                      {range.map(i => {
                        return (
                          <>
                            <Button size="lg" variant={i === nps ? "primary " : rangeColors[i]} onClick={() => { onClick(i) }} disabled={loading} style={{ marginBottom: 10 }}>{i}</Button>{' '}
                          </>
                        );
                      })}
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      {nps === null ? <p><b>Selecione</b> uma nota entre 0 pouco satisfeito, 10 muito satisfeito.</p> : <p>{`Você selecionou a nota ${nps}, escreva um comentário e clicar em confirmar e enviar.`}</p>}
                    </Col>
                  </Row>
                  <br />
                  <Row>
                    <Col xm={8}>
                      <Form>
                        <Form.Group controlId="comentario">
                          <Form.Label>Comentário adicional</Form.Label>
                          <Form.Control as="textarea" rows={3} onChange={comentarioOnChange} disabled={loading || nps === null} />
                        </Form.Group>
                        <Button variant="primary" type="Button" onClick={confirmOnClick} disabled={loading || nps === null}>
                          Confirmar e Enviar
                        </Button>
                      </Form>
                    </Col>
                  </Row>
                </>
              )
            }
          </Container>
        </>
      )
      }
    </div >
  );
}

export default NpsPage;