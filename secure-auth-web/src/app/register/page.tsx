/* eslint-disable @typescript-eslint/no-explicit-any */
'use client';

import { api } from '@/lib/api';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useState } from 'react';

export default function RegisterPAge() {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const [message, setMessage] = useState('');
	const [error, setError] = useState('');
	const router = useRouter();

	const handleRegister = async (e: React.FormEvent) => {
		e.preventDefault();
		setError('');
		setMessage('');

		try {
			await api.post('/auth/register', { email, password });
			setMessage('Account created successfully');
			setTimeout(() => router.push('/login'), 2000);
		} catch (error: any) {
			setError(error.response?.data?.message || 'Error to register!');
		}
	};

	return (
		<main className='flex min-h-screen items-center justify-center bg-gray-50 p-4'>
			<div className='bg-white p-8 rounded-lg shadow-md w-full max-w-md text-black'>
				<h1 className='text-2xl font-bold mb-6 text-center'>
					Criar Conta
				</h1>
				{error && (
					<p className='text-red-500 mb-4 text-center'>{error}</p>
				)}
				{message && (
					<p className='text-green-500 mb-4 text-center'>{message}</p>
				)}

				<form
					onSubmit={handleRegister}
					className='flex flex-col gap-4'
				>
					<input
						type='email'
						value={email}
						onChange={(e) => setEmail(e.target.value)}
						placeholder='Seu melhor e-mail'
						className='border p-2 rounded'
						required
					/>
					<input
						type='password'
						value={password}
						onChange={(e) => setPassword(e.target.value)}
						placeholder='Sua senha segura'
						className='border p-2 rounded'
						required
					/>
					<button
						type='submit'
						className='bg-green-600 text-white p-2 rounded hover:bg-green-700 transition'
					>
						Cadastrar
					</button>
				</form>
				<div className='mt-4 text-center'>
					<Link
						href='/login'
						className='text-blue-600 hover:underline'
					>
						Já tem uma conta? Faça login
					</Link>
				</div>
			</div>
		</main>
	);
}
