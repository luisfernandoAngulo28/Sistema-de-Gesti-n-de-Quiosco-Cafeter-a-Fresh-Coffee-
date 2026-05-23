import { createContext, useState, useEffect } from 'react'
import { toast } from 'react-toastify';
import clienteAxios from '../config/axios';

const QuioscoContext = createContext();

const QuioscoProvider = ({children}) => {

    const [categorias, setCategorias ] = useState([]);
    const [categoriaActual, setCategoriaActual] = useState({})
    const [modal, setModal] = useState(false)
    const [producto, setProducto] = useState({})
    const [numerMesa, setNumerMesa] = useState(() => {
        const guardada = localStorage.getItem('QUIOSCO_MESA')
        return guardada ? parseInt(guardada) : ''
    })
    const [pedido, setPedido] = useState(() => {
        try {
            const guardado = localStorage.getItem('QUIOSCO_PEDIDO')
            return guardado ? JSON.parse(guardado) : []
        } catch {
            return []
        }
    })
    const [total, setTotal] = useState(0)
    const [carritoAnimado, setCarritoAnimado] = useState(false)
    const [metodoPago, setMetodoPago] = useState('')
    const [productoRecienAgregado, setProductoRecienAgregado] = useState(null)

    useEffect(() => {
        const nuevoTotal = pedido.reduce( (total, producto) => (producto.precio * producto.cantidad) + total, 0 )
        setTotal(nuevoTotal)
        localStorage.setItem('QUIOSCO_PEDIDO', JSON.stringify(pedido))
    }, [pedido])

    useEffect(() => {
        if (numerMesa) localStorage.setItem('QUIOSCO_MESA', numerMesa)
        else localStorage.removeItem('QUIOSCO_MESA')
    }, [numerMesa])

    const obtenerCategorias = async () => {
        const token = localStorage.getItem('AUTH_TOKEN')
        try {
            const {data} = await clienteAxios('/api/categorias', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
            if (data.data && data.data.length > 0) {
                setCategorias(data.data)
                setCategoriaActual(data.data[0])
            }
        } catch (error) {
            console.log(error)
        }
    }

    useEffect(() => {
        obtenerCategorias();
    }, [])

    const handleClickCategoria = id => {
        const categoria = categorias.filter(categoria => categoria.id === id)[0]
        setCategoriaActual(categoria)
    }

    const handleClickModal = () => {
        setModal(!modal)
    }

    const handleSetProducto = producto => {
        setProducto(producto)
    }

    const handleAgregarPedido = ({categoria_id, ...producto}) => {
        if(pedido.some( pedidoState => pedidoState.id === producto.id )) {
            const pedidoActualizado = pedido.map( pedidoState => pedidoState.id === producto.id ? producto : pedidoState)
            setPedido(pedidoActualizado)
            toast.success('Cantidad actualizada')
        } else {
            setPedido([...pedido, producto])
            toast.success('¡Agregado al pedido!')
            setCarritoAnimado(true)
            setTimeout(() => setCarritoAnimado(false), 600)
        }
        setProductoRecienAgregado(producto.id)
        setTimeout(() => setProductoRecienAgregado(null), 1000)
    }

    const handleEditarCantidad = id => {
        const productoActualizar = pedido.filter(producto => producto.id === id)[0]
        setProducto(productoActualizar)
        setModal(!modal)
    }

    const handleEliminarProductoPedido = id => {
        const pedidoActualizado = pedido.filter(producto => producto.id !== id )
        setPedido(pedidoActualizado)
        toast.success('Eliminado del Pedido')
    }

    const handleSubmitNuevaOrden = async (navigate) => {
        const token = localStorage.getItem('AUTH_TOKEN')
        try {
            const { data } = await clienteAxios.post('/api/pedidos',
            {
                total,
                mesa_id: numerMesa || null,
                productos: pedido.map(producto => ({
                    id: producto.id,
                    cantidad: producto.cantidad
                }))
            },
            { headers: { Authorization: `Bearer ${token}` } })

            // Registrar el pago — el backend devuelve { id, message } y usa snake_case
            if (metodoPago && data?.id) {
                await clienteAxios.post('/api/pagos',
                    { pedido_id: data.id, metodo: metodoPago, monto: total },
                    { headers: { Authorization: `Bearer ${token}` } }
                )
            }

            // Guardar info para la página de gracias antes de limpiar
            const resumenPedido   = [...pedido]
            const resumenTotal    = total
            const resumenMesa     = numerMesa
            const resumenMetodo   = metodoPago

            // Limpiar carrito
            setPedido([])
            setNumerMesa('')
            setMetodoPago('')
            localStorage.removeItem('QUIOSCO_PEDIDO')
            localStorage.removeItem('QUIOSCO_MESA')

            navigate('/gracias', {
                state: {
                    pedido:      resumenPedido,
                    total:       resumenTotal,
                    numerMesa:   resumenMesa,
                    metodoPago:  resumenMetodo,
                }
            })
        } catch (error) {
            if (error?.response?.status === 401) {
                localStorage.removeItem('AUTH_TOKEN')
                window.location.href = '/auth/login'
            } else {
                toast.error('Error al procesar el pedido. Intenta de nuevo.')
            }
        }
    }

    const handleClickCompletarPedido = async id => {
        const token = localStorage.getItem('AUTH_TOKEN')
        try {
            await clienteAxios.put(`/api/pedidos/${id}`, null, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        } catch (error) {
            console.log(error)
        }
    }

    const handleClickProductoAgotado = async id => {
        const token = localStorage.getItem('AUTH_TOKEN')
        try {
            await clienteAxios.put(`/api/productos/${id}`, null, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
        } catch (error) {
            console.log(error)
        }
    }


    return (
        <QuioscoContext.Provider
            value={{
                categorias,
                categoriaActual,
                handleClickCategoria,
                modal,
                handleClickModal,
                producto,
                handleSetProducto,
                pedido,
                handleAgregarPedido,
                handleEditarCantidad,
                handleEliminarProductoPedido,
                total,
                numerMesa,
                setNumerMesa,
                carritoAnimado,
                metodoPago,
                setMetodoPago,
                productoRecienAgregado,
                handleSubmitNuevaOrden,
                handleClickCompletarPedido,
                handleClickProductoAgotado
            }}
        >{children}</QuioscoContext.Provider>
    )
}

export {
    QuioscoProvider
}
export default QuioscoContext