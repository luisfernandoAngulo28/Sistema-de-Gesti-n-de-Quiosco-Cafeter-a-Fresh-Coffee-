import { Outlet } from 'react-router-dom'
import Modal from 'react-modal'
import { ToastContainer } from 'react-toastify'
import "react-toastify/dist/ReactToastify.css";
import Sidebar from '../components/Sidebar'
import Resumen from '../components/Resumen'
import ModalProducto from '../components/ModalProducto'
import useQuisco from '../hooks/useQuiosco'
import { useAuth } from '../hooks/useAuth';

const customStyles = {
  content: {
    top: "50%",
    left: "50%",
    right: "auto",
    bottom: "auto",
    marginRight: "-50%",
    transform: "translate(-50%, -50%)",
    padding: "0",
    borderRadius: "1rem",
    border: "none",
    boxShadow: "0 20px 60px rgba(0,0,0,0.15)",
    overflow: "hidden",
    maxWidth: "680px",
    width: "95%",
  },
  overlay: {
    backgroundColor: "rgba(0, 0, 0, 0.5)",
    backdropFilter: "blur(2px)",
    zIndex: 50,
  }
};

Modal.setAppElement('#root')

export default function Layout() {
  useAuth({middleware: 'auth'})
  const { modal } = useQuisco();

  return (
    <>
        <div className='md:flex bg-slate-50 dark:bg-gray-950 min-h-screen transition-colors duration-200'>
          <Sidebar />

          <main className='flex-1 h-screen overflow-y-scroll p-6 bg-slate-50 dark:bg-gray-950 transition-colors duration-200'>
            <Outlet />
          </main>

          <Resumen />
        </div>

        <Modal isOpen={modal} style={customStyles}>
            <ModalProducto />
        </Modal>

        <ToastContainer theme="colored" />
    </>
  )
}
